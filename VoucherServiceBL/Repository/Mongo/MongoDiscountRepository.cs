using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoDiscountRepository : IDiscountRepository
    {
        public Task<int> CreateDiscountVoucher(Discount discount)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Discount>> GetAllDiscountVouchersFilterByMerchantId(string merchantId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Discount> GetDiscountVoucher(Voucher voucher)
        {
            throw new System.NotImplementedException();
        }
    }
}