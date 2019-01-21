using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.ValueVoucher.Repository;

namespace VoucherServiceBL.Service
{
    /// <summary>
    /// A interface that handles the management of a value voucher
    /// </summary>
    

    public interface IValueVoucher
    {
        Value CreateValueVoucher(VoucherRequest value);

        List<Value> GetAllValueVouchers(string merchantId);

        Value GetValueVoucher(Voucher voucher);
    }
}