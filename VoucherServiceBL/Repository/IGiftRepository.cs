
using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    /// <summary>
    /// An interface that interacts with the database concerning a Gift voucher
    /// </summary>
    public interface IGiftRepository
    {
        IEnumerable<Gift> GetAllGiftVouchers(string merchantId);

        Gift CreateGiftVoucher(Gift voucher);

        /// <summary>
        /// Upward review of the amount on a gift voucher
        /// A gift voucher's amount cannot be reduced after
        /// it has been created
        /// </summary>
        /// <param name="amountToAdd">Amount to add to the current balance on the gift voucher</param>
        /// <returns>The gift voucher</returns>
        Voucher UpdateGiftVoucherAmount(string code, Gift voucher); //TODO: decide to either return the modified voucher or void
        Gift GetGiftVoucher(Voucher voucher);
    }
}