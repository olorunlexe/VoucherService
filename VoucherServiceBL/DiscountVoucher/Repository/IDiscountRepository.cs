using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.DiscountVoucher
{
    interface IDiscountRepository
    {
        Discount CreateDiscountVoucher(string merchant);
        Discount DeleteDiscountVoucher(BigInteger id, string merchant);

        Discount GetDiscountVoucherByIdFilterByMerchantId(long id, string merchantId);
        Discount GetAllDiscountVoucherByMerchantId(string merchant);
        Discount GetDiscountVoucherByCodeFilterByMerchantId(long code, string merchantId);
        Discount GetDiscountVoucherByExpiryDate(string code);
        Discount GetDiscountVoucherByCreationDate(string code);
        Discount GetDiscountVoucherByCode(long code, string merchantId);
        Discount GetAllDiscountVoucherByStatus(string status,string merchantId);

        Discount UpdateDiscountVoucher(BigInteger vid, Discount discount);

    }
}
