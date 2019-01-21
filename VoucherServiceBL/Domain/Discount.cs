using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace VoucherServiceBL.Domain
{
    [Table("DiscountVoucher")]
    public class Discount:Voucher,ISingleVoucher
    {
        public long DiscountAmount { get; set; }
        public long DiscountUnit { get; set; }
        public float DiscountPercent { get; set; }
        public long RedemptionCount { get; set; }
    }
}
