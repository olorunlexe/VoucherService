using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.Domain
{
    [Table("Voucher")]
    public class Voucher
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string VoucherType { get; set; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime ExpiryDate { get; set; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreationDate { get; set; }
        public string MerchantId { get; set; }
        public string VoucherStatus { get; set; }
        public string Metadata { get; set; }
        public string Description { get; set; }

    }
}
