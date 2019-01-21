using System;
using System.Collections.Generic;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;

namespace VoucherServiceBL.Service
{
    public interface IVoucherService
    {
        Voucher CreateVoucher(VoucherRequest voucherRequest);
        Voucher GetVoucherByCode(string code);
        IEnumerable<Voucher> GetAllVouchers(string merchantId); 
        void DeleteVoucher(string code);
        Voucher ActivateOrDeactivateVoucher(string code);
        Voucher UpdateGiftVoucherAmount(string code, long amount);
        Voucher UpdateVoucherExpiryDate(string code, DateTime newDate);

        IEnumerable<Gift> GetAllGiftVouchers(string merchantId);
         Gift GetGiftVoucher(string code);

        Value GetValueVoucher(string code);
        IEnumerable<Value> GetAllValueVouchers(string merchantId);

        IEnumerable<Discount> GetAllDiscountVouchers(string merchantId);
        Discount GetDiscountVoucher(string code);

        //Rest of common methods
    }
}