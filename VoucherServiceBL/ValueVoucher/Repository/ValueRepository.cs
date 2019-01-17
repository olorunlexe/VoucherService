using System;
using System.Collections.Generic;
using System.Text;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.ValueVoucher.Repository
{
    public class ValueRepository : BaseRepository, IValueRepository
    {
        public Value CreateValueVoucher(Value voucher)
        {
            using (var connection = Connection)
            {
                if (connection.Closed) connection.open();

            }
        }

        public IEnumerable<Value> GetAllValueVouchers(string merchantId)
        {
            throw new NotImplementedException();
        }
    }
}
