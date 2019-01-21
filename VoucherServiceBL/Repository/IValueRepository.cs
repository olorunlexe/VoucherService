using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.ValueVoucher.Repository
{
    public interface IValueRepository
    {
   
        #region Create Method

        /// <summary>
        /// Create a voucher from a given code
        /// </summary>
        /// <param name="code">the code to create a voucher from</param>
        /// <returns>a single voucher</returns>   
        Value CreateValueVoucher(Value value);

        #endregion

        #region Read Method 

         /// <summary>
        /// Returns all value vouchers
        /// filtered by the id of the merchant that created the value voucher///
        /// </summary>
        /// <param name="merchantId">id of the merchant that created the voucher</param>
        /// <returns>a list of value vouchers</returns>
        List<Value> GetAllValueVouchers(Value value);

        #endregion


    }

}