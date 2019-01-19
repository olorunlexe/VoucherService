using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace VoucherServiceBL.Domain
{
    [Table("ValueVoucher")]
    public class Value:Voucher
    {
        public long valueAmount { get; set; }
        public string status { get; set; }
    }
}
