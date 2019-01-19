using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.ValueVoucher.Repository
{
    public class ValueRepositoryImpl : BaseRepository, IValueRepository
    {
       public ValueRepositoryImpl(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// Create Value Voucher repository handler
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Value CreateValueVoucher(Value value)
        {        
                var rowAffected = 0;
                using (var conn = Connection)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@HashedCode", value.Code);
                    parameters.Add("@MerchantId", value.MerchantId);
                    parameters.Add("@ValueAmount", value.ValueAmount);
                    parameters.Add(" @ExpiryDate", value.ExpiryDate);

                    rowAffected = conn.Execute("usp_CreateVoucher", parameters, commandType: CommandType.StoredProcedure);
                }
                return value;
        }


        /// <summary>
        /// Read All Value Vouchers filtered by a MerchantId From Table handler
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<Value> GetAllValueVouchers(Value value)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllValueVouchers"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchanttId", value.MerchantId);
                return conn.Query<Value>("usp_GetAllValueVouchersFilterByMerchantId",parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
