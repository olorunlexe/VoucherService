using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace VoucherServiceBL.Domain
{
    [Table("DiscountVoucher")]
    public class Discount:Voucher, ISingleVoucher
    {
        public DiscountType discountType;
        public long Amount { get; set; }
        public long Unit { get; set; }
        public float Percent { get; set; }
        public BigInteger VoucherId { get; set; }
        public BigInteger RedemptionCount { get; set; }
    }
}
