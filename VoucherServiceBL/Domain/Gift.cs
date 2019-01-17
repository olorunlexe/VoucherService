using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace VoucherServiceBL.Domain
{
    [Table("GiftVoucher")]
    public class Gift:Voucher
    {
        public BigInteger giftAmount { get; set; }
        public BigInteger giftUnit { get; set; }
        public string status { get; set; }
    }
}
