﻿using System;
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
        Task<int> CreateDiscountVoucher( VoucherRequest voucherRequest);

        Task<Discount> GetDiscountVoucher(Voucher voucher);
        //Discount GetAllDiscountVouchers();
        Task<IEnumerable<Discount>> GetAllDiscountVouchersFilterByMerchantId(string merchantId);
    }
}
