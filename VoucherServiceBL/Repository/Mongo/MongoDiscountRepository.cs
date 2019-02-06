using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoDiscountRepository :BaseMongoRepository, IDiscountRepository
    {

        private IMongoCollection<Discount> _discountVoucher;
        public MongoDiscountRepository(MongoClient client, IConfiguration config):base(client, config)
            =>
            _discountVoucher = _database.GetCollection<Discount>(nameof(_discountVoucher));


        public async Task<int> CreateDiscountVoucherAsync(Discount discount)
        {
            await _vouchers.InsertOneAsync(discount as Voucher);
            await _discountVoucher.InsertOneAsync(discount);
            return 1;
        }

        public async Task<int> CreateDiscountVoucherAsync(IList<Discount> vouchersList)
        {
            await _vouchers.InsertManyAsync(vouchersList as IList<Voucher>);
            await _discountVoucher.InsertManyAsync(vouchersList);
            return vouchersList.Count;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountVouchersFilterByMerchantIdAsync(string merchantId)
        {
            var vouchers = await _discountVoucher.FindAsync(v =>  v.MerchantId == merchantId);
            return await vouchers.ToListAsync();
        }

        public async Task<Discount> GetDiscountVoucherAsync(Voucher voucher)
        {
            var discountVoucher = await _discountVoucher.FindAsync( v => 
                    v.Code == voucher.Code && v.MerchantId == voucher.MerchantId
            );

            return await discountVoucher.FirstAsync();
        }
    }
}