using System;
using System.Numerics;

namespace VoucherServiceBL.Service
{
    public class VoucherUpdateReq
    {
        public string Status { get; set; }
        public DateTime ExpiryDate { get; set; }
        public BigInteger GiftAmount { get; set; }
        public string Code { get; set; }
    }
}