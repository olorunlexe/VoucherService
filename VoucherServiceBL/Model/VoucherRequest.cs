using System;
using System.Numerics;

namespace VoucherServiceBL.Model
{
    public class VoucherRequest
    {
        public string VoucherType { get; set; }
        public long VoucherAmount { get; set; }
        public long DiscountAmount { get; set; }
        public int DiscountUnit { get; set; }
        public int DiscountPercent { get; set; }
        public long GiftAmount { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string CodePattern { get; set; }
        public int CodeLength { get; set; }
        public string CharacterSet { get; set; }
        public string Separator { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Description { get; set; }
        public string Metadata;
        public int NumbersOfVoucherToCreate { get; set; }
        public string MerchantId { get; set; }
    }
}