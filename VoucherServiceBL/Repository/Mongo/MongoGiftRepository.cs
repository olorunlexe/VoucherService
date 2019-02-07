using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoGiftRepository : BaseMongoRepository, IGiftRepository
    {
        private IMongoCollection<Gift> _giftVouchers;
        private ILogger<MongoGiftRepository> _logger;
        
        public MongoGiftRepository(MongoClient client, IConfiguration config, ILogger<MongoGiftRepository> logger):base(client, config) 
        { 
            _giftVouchers = _database.GetCollection<Gift>(nameof(_giftVouchers));
            _logger = logger;
        }
        public async Task<int> CreateGiftVoucherAsync(Gift voucher)
        {
            await _vouchers.InsertOneAsync(voucher as Voucher);
            await _giftVouchers.InsertOneAsync(voucher);
            return 1;
        }

        public async Task<int> CreateGiftVoucherAsync(IList<Gift> vouchersList)
        {
            await _vouchers.InsertManyAsync(vouchersList);
            await _giftVouchers.InsertManyAsync(vouchersList);
            return vouchersList.Count;
        }

        public async Task<IEnumerable<Gift>> GetAllGiftVouchersAsync(string merchantId)
        {
            var cursor = await _giftVouchers.FindAsync( v => v.MerchantId == merchantId);
            return await cursor.ToListAsync();
        }

        public async Task<Gift> GetGiftVoucherAsync(Voucher voucher)
        {
            var cursor = await _giftVouchers.FindAsync( v => 
                v.Code == voucher.Code && v.MerchantId == voucher.MerchantId );
            return await cursor.FirstAsync();
        }

        public async Task<int?> UpdateGiftVoucherAmountAsync(Gift voucher)
        {
            //get the voucher to update first
            //to check the previous balance and add to it
            var voucherToUpdate = await GetGiftVoucherAsync(voucher);
            if (voucherToUpdate == null)
            {
                _logger.LogInformation("Problem occurred updating the voucher");
                return null;
            }
            voucherToUpdate.GiftBalance += voucher.GiftBalance;
            voucherToUpdate.GiftAmount += voucher.GiftBalance;

            var filter = Builders<Gift>.Filter.Eq("code", voucherToUpdate.Code);
            var updateDef = Builders<Gift>.Update.Set("gift_balance", voucherToUpdate.GiftBalance)
                                                 .Set("gift_amount", voucherToUpdate.GiftAmount);       ;

            var cursor = await _giftVouchers.UpdateOneAsync(filter, updateDef);

            return (int) cursor.ModifiedCount; //FIXME: casting long to int TODO: may throw execption handle
        }
    }
}