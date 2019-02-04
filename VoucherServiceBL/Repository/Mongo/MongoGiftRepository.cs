using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoGiftRepository : IGiftRepository
    {
        public Task<int> CreateGiftVoucher(Gift voucher)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CreateGiftVoucher(IEnumerable<Gift> vouchersList)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Gift> GetGiftVoucher(Voucher voucher)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateGiftVoucherAmount(Gift voucher)
        {
            throw new System.NotImplementedException();
        }
    }
}