using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherServiceBL.DiscountVoucher
{
    interface IDiscountVoucher
    {
        IDiscountRepository repository { get; set; }
    }
}
