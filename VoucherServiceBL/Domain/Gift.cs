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
<<<<<<< HEAD
        public BigInteger GiftBalance { get; set; }
=======
        public BigInteger GiftUnit { get; set; }
        public string Status { get; set; }
>>>>>>> 19b5ad66dfb6cdca8d178c29eb70084645ce64a1
=======
        public BigInteger GiftBalance { get; set; }
>>>>>>> 57b5f2e70cb83b909bc8d274a6258f7757e9e1cf
    }
}
