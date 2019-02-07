using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
<<<<<<< HEAD
using VoucherServiceBL.Util;
=======
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
>>>>>>> 66e3cfe2e85fb37589e6fc8f65d77c65c3e38eb9

namespace VoucherServiceBL.Domain
{
    [Table("Voucher")]
    public class Voucher
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId _id { get; set; }

        [BsonIgnore]
        public long Id { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("voucher_type")]
        public string VoucherType { get; set; }
<<<<<<< HEAD
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime ExpiryDate { get; set; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
=======

        [BsonElement("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [BsonElement("creation_date")]
>>>>>>> 66e3cfe2e85fb37589e6fc8f65d77c65c3e38eb9
        public DateTime CreationDate { get; set; }

        [BsonElement("merchant_id")]
        public string MerchantId { get; set; }

        [BsonElement("voucher_status")]
        public string VoucherStatus { get; set; }

        [BsonElement("Metadata")]
        public string Metadata { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

    }
}
