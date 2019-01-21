using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Repository;

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

                    //Parameters Declaration to be passed into Stored procdure "usp_CreateVoucher"..
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
        public List<Value> GetAllValueVouchers(string merchantId)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllValueVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);
                return conn.Query<Value>("usp_GetAllValueVouchersFilterByMerchantId",parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Read All details of a Value Voucher filtered by a MerchantId From Table handler
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        public Value GetValueVoucher(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByCodeFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);
                return conn.QuerySingle<Value>("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
