using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;
using VoucherServiceBL.ValueVoucher.Repository;

namespace VoucherServiceBL.ValueVoucher
{
    /// <summary>
    /// A interface that handles the management of a value voucher
    /// </summary>
    

    public interface IValueVoucher
{
    IValueRepository Repository { get; set; }

        #region Create Methods

        /// <summary>
        /// Create a voucher from a given code
        /// </summary>
        /// <param name="code">the code to create a voucher from</param>
        /// <returns>a single voucher</returns>            
        IEnumerable<Value> CreateValueVoucher(IEnumerable<string> codes);

        /// <summary>
        /// Create multiple vouchers at once given a list of codes
        /// </summary>
        /// <param name="codes">a list of codes</param>
        /// <returns>an immutable list of vouchers</returns>
        Value CreateValueVoucher(string code);

        #endregion

        #region Read Methods          
        Value GetValueVoucherById(BigInteger id, string merchantId);

        /// <summary>
        /// Returns a value voucher given a voucher code
        /// filtered by the id of the merchant that created the voucher///
        /// </summary>
        /// <param name="code">code of the voucher</param>
        /// <param name="merchantId">id of the merchant that created the voucher</param>
        /// <returns></returns>
        Value GetValueVoucherByCode(string code, string merchantId);

        /// <summary>
        /// Returns a value voucher given a voucher code
        /// filtered by the id of the merchant that created the voucher///
        /// </summary>
        /// <param name="creationDate">id of the merchant that created the voucher</param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Value GetValueVoucherByCreationDate(DateTime creationDate, string merchantId);

        Value GetValueVoucherByExpiryDate(DateTime expiryDate, string merchantId);
        Value GetValueVoucherByStatus(string status, string merchantId);
        IEnumerable<Value> GetAllValueVouchers(string merchantId);
        #endregion
    }
}