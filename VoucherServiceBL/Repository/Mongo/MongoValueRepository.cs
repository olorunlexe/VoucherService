using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoValueRepository : BaseMongoRepository, IValueRepository
    {
        private ILogger<Value> _logger;
        public MongoValueRepository(MongoClient client, IConfiguration config, ILogger<Value> logger):base(client, config)
            {
                _logger = logger;
                // BsonClassMap.RegisterClassMap<Value>();

            }
        public async Task<int> CreateValueVoucherAsync(Value value)
        {
            await _vouchers.InsertOneAsync(value);
            return 1;
        }
        public async  Task<int> CreateValueVoucherAsync(IList<Value> vouchersList)
        {
            await _vouchers.InsertManyAsync(vouchersList);            
            return vouchersList.Count;
        }

        public async Task<IEnumerable<Value>> GetAllValueVouchersAsync(string merchantId)
        {
            var filter = Builders<Voucher>.Filter.Eq("merchant_id", merchantId);
            var cursor = await _vouchers.FindAsync( filter);

            var vouchers = await cursor.ToListAsync();
            IList<Value> valueVouchers = new List<Value>();// .FirstOrDefault() as Value};
            vouchers.ForEach(v => valueVouchers.Add(v as Value));
            return valueVouchers;
        }

        public async Task<Value> GetValueVoucherAsync(Voucher voucher)
        {
            var voucherCursor = await _vouchers.FindAsync( v => 
                    v.Code == voucher.Code && v.MerchantId == voucher.MerchantId &&
                    v.VoucherType == voucher.VoucherType);
            return await voucherCursor.FirstOrDefaultAsync() as Value ;
        }
    }
}