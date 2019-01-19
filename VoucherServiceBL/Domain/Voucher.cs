using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace VoucherServiceBL.Domain
{
    [Table("Voucher")]
    public class Voucher
    {
        public BigInteger Id { get; set; }
        public string Code { get; set; }
        public string VoucherType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string MerchantId { get; set; }
        public string VoucherStatus { get; set; }
        public string Metadata { get; set; }
        public string Description { get; set; }

    }
}
