
using System;
using System.Collections.Generic;
using System.Numerics;

namespace VoucherServiceBL.GiftVoucher.Repository
{
    /// <summary>
    /// An interface that interacts with the database concerning a Gift voucher
    /// </summary>
    public interface IGiftRepository
    {
        GiftVoucher GetGiftVoucherById(BigInteger id, string merchantId);

        GiftVoucher GetGiftVoucherByCode(string code, string merchantId);
        
        GiftVoucher GetGiftVoucherByCreationDate(DateTime creationDate, string merchantId);
        
        GiftVoucher GetGiftVoucherByExpiryDate(DateTime expiryDate, string merchantId);
        GiftVoucher GetGiftVoucherByStatus(string status, string merchantId);
        IEnumerable<GiftVoucher> GetAllGiftVouchers(string merchantId);

        GiftVoucher CreateGiftVoucher(GiftVoucher voucher);

        /// <summary>
        /// Upward review of the amount on a gift voucher
        /// A gift voucher's amount cannot be reduced after
        /// it has been created
        /// </summary>
        /// <param name="amountToAdd">Amount to add to the current balance on the gift voucher</param>
        /// <returns>The gift voucher</returns>
        void UpdateGiftVoucherAmount(BigInteger id, GiftVoucher voucher); //TODO: decide to either return the modified voucher or void

        void usp_DeleteVoucher(BigInteger id);

// [dbo].[usp_DeleteVoucherByCode]
// [dbo].[usp_DeleteVoucherById]      
    }
}