using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Repository
{
    public class DiscountRepository : BaseRepository,IDiscountRepository
    {
        public DiscountRepository(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// Create Discount Voucher repository handler
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        public Discount CreateDiscountVoucher(Discount discount)
        {        
                var rowAffected = 0;
                using (var conn = Connection)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@HashedCode", discount.Code);
                    parameters.Add("@MerchantId", discount.MerchantId);
                    parameters.Add("@DiscountAmount", discount.DiscountAmount);
                    parameters.Add("@DiscountPercent", discount.DiscountPercent);
                    parameters.Add("@DiscountUnit", discount.DiscountUnit);
                    parameters.Add("@ExpiryDate", discount.ExpiryDate);

                    rowAffected = conn.Execute("usp_CreateDiscountVoucher", parameters, commandType: CommandType.StoredProcedure);
                }
                return discount;
        }


        /// <summary>
        /// Read Discount Voucher From Table handler
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        public IEnumerable<Discount> GetAllDiscountVouchersFilterByMerchantId(string merchantId)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchanttId", merchantId);
                return conn.Query<Discount>("usp_usp_GetAllDiscountVouchersFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Discount GetDiscountVoucher(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);
                return conn.QuerySingle<Discount>("usp_GetVoucherByCodeFilterByMerchantId", commandType: CommandType.StoredProcedure);
            }
        }
    }

}
