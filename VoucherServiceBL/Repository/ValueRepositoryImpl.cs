using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<int> CreateValueVoucher(Value value)
        {        
                using (var conn = Connection)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    //Parameters Declaration to be passed into Stored procdure "usp_CreateVoucher"..
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@HashedCode", value.Code);
                    parameters.Add("@MerchantId", value.MerchantId);
                    parameters.Add("@ValueAmount", value.ValueAmount);
                    parameters.Add("@ExpiryDate", value.ExpiryDate);

                    return await conn.ExecuteAsync("usp_CreateValueVoucher", parameters, commandType: CommandType.StoredProcedure);
                }
        }


        /// <summary>
        /// Read All Value Vouchers filtered by a MerchantId From Table handler
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllValueVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);
                return await conn.QueryAsync<Value>("usp_GetAllValueVouchersFilterByMerchantId",parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Read All details of a Value Voucher filtered by a MerchantId From Table handler
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        public async Task<Value> GetValueVoucher(Voucher voucher)
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
                return await conn.QuerySingleAsync<Value>("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
