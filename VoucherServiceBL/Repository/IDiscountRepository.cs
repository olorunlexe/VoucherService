﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    public interface IDiscountRepository
    {
        Discount CreateDiscountVoucher(Discount discount);
        Discount GetDiscountVoucher(Voucher voucher);
        IEnumerable<Discount> GetAllDiscountVouchersFilterByMerchantId(string merchantId);

    }

}
