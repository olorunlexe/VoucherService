using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace VoucherServiceBL.DiscountVoucher
{
    interface IDiscountRepository
    {
        DiscountVoucher CreateDiscountVoucher(string merchant);
        DiscountVoucher DeleteDiscountVoucher(BigInteger id, string merchant);

        DiscountVoucher GetDiscountVoucherByIdFilterByMerchantId(long id, string merchantId);
        DiscountVoucher GetAllDiscountVoucherByMerchantId(string merchant);
        DiscountVoucher GetDiscountVoucherByCodeFilterByMerchantId(long clode, string merchantId);
        DiscountVoucher GetDiscountVoucherByExpiryDate(string code);
        DiscountVoucher GetDiscountVoucherByCreationDate(string code);
        DiscountVoucher GetDiscountVoucherByCode(long code, string merchantId);
        DiscountVoucher GetAllDiscountVoucherByStatus(string status,string merchantId);

        DiscountVoucher UpdateDiscountVoucher(BigInteger vid,DiscountVoucher discount);

    }
}
