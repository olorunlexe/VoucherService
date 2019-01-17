using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.DiscountVoucher
{
    interface IDiscountRepository
    {
        Discount CreateDiscountVoucher(Discount discount);
        //Discount GetAllDiscountVouchers();
        List<Discount> GetAllDiscountVouchersFilterByMerchantId(Discount discount);
    }
}
