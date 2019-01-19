
using System.Collections.Generic;
using VoucherServiceBL.Repository;
using VoucherServiceBL.Domain;
using System.Numerics;
using VoucherServiceBL.Model;

namespace VoucherServiceBL.Service
{
    /// <summary>
    /// A interface that handles the management of a gift voucher
    /// </summary>
    public interface IGiftVoucher
    {
        Gift CreateGiftVoucher(VoucherRequest giftRequest);

        Gift GetGiftVoucher(Voucher voucher);

        Voucher UpdateGiftVoucher(Gift giftVoucher);
    }
}