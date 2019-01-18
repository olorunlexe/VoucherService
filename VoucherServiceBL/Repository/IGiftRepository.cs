
using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.GiftVoucher.Repository
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
        void UpdateGiftVoucherAmount(BigInteger id, Gift voucher); //TODO: decide to either return the modified voucher or void

        // void usp_DeleteVoucher(BigInteger id);

                // public Gift GetGiftVoucherByCode(string code, string merchantId)
        // {
        //     throw new NotImplementedException();
        // }

        // public Gift GetGiftVoucherByCreationDate(DateTime creationDate, string merchantId)
        // {
        //     throw new NotImplementedException();
        // }

        // public Gift GetGiftVoucherByExpiryDate(DateTime expiryDate, string merchantId)
        // {
        //     throw new NotImplementedException();
        // }

        // public Gift GetGiftVoucherById(BigInteger id, string merchantId)
        // {
        //     throw new NotImplementedException();
        // }

        // public Gift GetGiftVoucherByStatus(string status, string merchantId)
        // {
        //     throw new NotImplementedException();
        // }

// [dbo].[usp_DeleteVoucherByCode]
// [dbo].[usp_DeleteVoucherById]      
    }
}