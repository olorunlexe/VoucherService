using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.ValueVoucher.Repository
{
    public interface IValueRepository
    {
   


        IEnumerable<Value> GetAllValueVouchers(string merchantId);

        Value CreateValueVoucher(Value voucher);

  

}