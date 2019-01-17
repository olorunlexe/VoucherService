using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.ValueVoucher.Repository
{
    public interface IValueRepository
    {
        Value GetValueVoucherById(BigInteger id, string merchantId);
        Value GetValueVoucherByCode(string code, string merchantId);
        Value GetValueVoucherByCreationDate(DateTime creationDate, string merchantId);
        Value GetValueVoucherByExpiryDate(DateTime expiryDate, string merchantId);
        Value GetValueVoucherByStatus(string status, string merchantId);


        IEnumerable<Value> GetAllValueVouchers(string merchantId);

        Value CreateValueVoucher(Value voucher);

        void UpdateValueVoucherExpiryDate(BigInteger id, Value valueVoucher);
        void UpdateValueVoucherStatus(BigInteger id, Value valueVoucher);

        void DeleteValueVoucher(BigInteger id);
    }

}