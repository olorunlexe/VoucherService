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
        public BigInteger ValueAmount { get; set; }
    }
}
