using System;
using System.Collections.Generic;
using System.Text;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public class DiscountVoucher : IDiscountVoucher
    {
        public IDiscountRepository discountRepository;

        public DiscountVoucher(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }


        public Discount CreateDiscountVoucher(VoucherRequest voucherRequest)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Discount> GetAllDiscountVouchersFilterByMerchantId(VoucherRequest voucherRequest)
        {
            throw new NotImplementedException();
        }
    }
}
