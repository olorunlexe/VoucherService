using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public interface IDiscountVoucherService
    {
        Discount CreateDiscountVoucher( VoucherRequest voucherRequest);

        Discount GetDiscountVoucher(Voucher voucher);
        //Discount GetAllDiscountVouchers();
        IEnumerable<Discount> GetAllDiscountVouchersFilterByMerchantId(string merchantId);
    }
}
