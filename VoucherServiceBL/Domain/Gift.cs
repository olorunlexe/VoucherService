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
        public BigInteger GiftAmount { get; set; }
<<<<<<< HEAD
        public BigInteger GiftBalance { get; set; }
=======
        public BigInteger GiftUnit { get; set; }
        public string Status { get; set; }
>>>>>>> 19b5ad66dfb6cdca8d178c29eb70084645ce64a1
    }
}
