using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace VoucherServiceBL.Domain
{
    public interface ISingleVoucher
    {
        long RedemptionCount { get; set; }
    }
}
