using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.DiscountVoucher
{
    interface IDiscountVoucher
    {
        IDiscountRepository repository { get; set; }

        Discount CreateDiscountVoucher(Discount discount);
        //Discount GetAllDiscountVouchers();
        IEnumerable<Discount> GetAllDiscountVouchersFilterByMerchantId(Discount discount);
    }
}
