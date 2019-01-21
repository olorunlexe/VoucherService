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
        Voucher UpdateVoucherExpiryDate(Voucher voucher);
        Voucher UpdateVoucher(VoucherUpdateReq voucher);
        void DeleteVoucher(string code);

     //Rest of common methods
    }
}