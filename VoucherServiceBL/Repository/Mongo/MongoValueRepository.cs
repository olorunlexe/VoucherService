using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.ValueVoucher.Repository;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoValueRepository : IValueRepository
    {
        public Task<int> CreateValueVoucher(Value value)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Value> GetValueVoucher(Voucher voucher)
        {
            throw new System.NotImplementedException();
        }
    }
}