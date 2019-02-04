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
        public long GiftAmount { get; set; }
        public long GiftBalance { get; set; }
        public long VoucherId { get; set; }
    }
}
