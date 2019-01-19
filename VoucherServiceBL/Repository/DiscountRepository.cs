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
                    parameters.Add("@DiscountAmount", discount.Amount);
                    parameters.Add("@DiscountPercent", discount.Percent);
                    parameters.Add("@DiscountUnit", discount.Unit);
                    parameters.Add(" @ExpiryDate", discount.ExpiryDate);

                    rowAffected = conn.Execute("usp_CreateDiscountVoucher", parameters, commandType: CommandType.StoredProcedure);
                }
                return discount;
        }

        //public List<Discount> GetAllDiscountVouchers()
        //{

        //    using (var conn = Connection)
        //    {
        //        if (conn.State == ConnectionState.Closed)
        //            conn.Open();

        //            return conn.Query<Discount>("usp_GetAllDiscountVouchers",commandType: CommandType.StoredProcedure).ToList();
        //    }

        //}



        /// <summary>
        /// Read Discount Voucher From Table handler
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        public IEnumerable<Discount> GetAllDiscountVouchersFilterByMerchantId(Discount discount)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchanttId", discount.MerchantId);
                return conn.Query<Discount>("usp_GetAllDiscountVouchers",parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }

}
