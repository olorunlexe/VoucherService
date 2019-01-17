
using System.Collections.Generic;
using VoucherServiceBL.GiftVoucher.Repository;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.GiftVoucher
{
    /// <summary>
    /// A interface that handles the management of a gift voucher
    /// </summary>
    public interface IGiftVoucher
    {
        IGiftRepository Repository { get; set; }    

        #region Create Methods

        /// <summary>
        /// Create a voucher from a given code
        /// </summary>
        /// <param name="code">the code to create a voucher from</param>
        /// <returns>a single voucher</returns>            
        IEnumerable<Gift> CreateGiftVoucher(IEnumerable<string> codes);   
        
        /// <summary>
        /// Create multiple vouchers at once given a list of codes
        /// </summary>
        /// <param name="codes">a list of codes</param>
        /// <returns>an immutable list of vouchers</returns>
        Gift CreateGiftVoucher(string code);

        #endregion

        #region Read Methods          
        GiftVoucher GetGiftVoucherById(BigInteger id, string merchantId);

        /// <summary>
        /// Returns a gift voucher given a voucher code
        /// filtered by the id of the merchant that created the voucher/// 
        /// </summary>
        /// <param name="code">code of the voucher</param>
        /// <param name="merchantId">id of the merchant that created the voucher</param>
        /// <returns></returns>
        GiftVoucher GetGiftVoucherByCode(string code, string merchantId);
        
        /// <summary>
        /// Returns a gift voucher given a voucher code
        /// filtered by the id of the merchant that created the voucher///
        /// </summary>
        /// <param name="creationDate">id of the merchant that created the voucher</param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        GiftVoucher GetGiftVoucherByCreationDate(DateTime creationDate, string merchantId);
        
        GiftVoucher GetGiftVoucherByExpiryDate(DateTime expiryDate, string merchantId);
        GiftVoucher GetGiftVoucherByStatus(string status, string merchantId);
        IEnumerable<GiftVoucher> GetAllGiftVouchers(string merchantId);
        #endregion
    }

}