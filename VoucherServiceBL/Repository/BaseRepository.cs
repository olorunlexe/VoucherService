using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Service;

namespace VoucherServiceBL.Repository
{
    public class BaseRepository
    {
        private static IConfiguration _config;

        public BaseRepository(IConfiguration config) => _config = config;

        public static IDbConnection Connection
        {
            get { return new SqlConnection(_config.GetConnectionString("MainConnString")); }
        }
         
         
       
        // public IEnumerable<Voucher> GetAllVouchers()
        /// {
        //     using (var conn = Connection)
        //     {
        //         if (conn.State == ConnectionState.Closed)
        //             conn.Open();

        //         return conn.Query<Voucher>("usp_GetAllVouchers", commandType: CommandType.StoredProcedure).ToList();
        //     }
        // }

        public async Task<IEnumerable<Voucher>> GetAllVouchersFilterByMerchantId(string merchantId)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);
                return await conn.QueryAsync<Voucher>("usp_GetAllVouchersFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByCode(string code)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", code);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByCode",parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByCodeFilterByMerchantId(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                // parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);
                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        

        public async Task<Voucher> GetVoucherByCreationDate(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
        //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByCreationDate"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CreationDate", voucher.CreationDate);
                parameters.Add("@VoucherType", voucher.VoucherType);
                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByCreationDate",parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByCreationDateFilterByMerchantId(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByCreationDate"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CreationDate", voucher.CreationDate);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByCreationDateFilterByMerchantId",parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByExpiryDate(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByExpiryDate"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ExpiryDate", voucher.CreationDate);
                parameters.Add("@VoucherType", voucher.VoucherType);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByExpiryDate", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByExpiryDateFilterByMerchantId(Voucher voucher)
        {

            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByExpiryDateFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ExpiryDate", voucher.ExpiryDate);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByExpiryDateFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherById(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherById"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherId", voucher.Id);
                parameters.Add("@VoucherType", voucher.VoucherType);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByIdFilterByMerchantId(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByIdFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherId", voucher.Id);
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByIdFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public async Task<Voucher> GetVoucherByMerchantId(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByStatus(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByStatus"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@VoucherStatus", voucher.VoucherStatus);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByStatus", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<int> UpdateVoucherExpiryDateByCode(Voucher voucher)
        {
            
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_UpdateVoucherExpiryDateByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@ExpiryDate", voucher.ExpiryDate);

                return await conn.ExecuteAsync("usp_UpdateVoucherExpiryDateByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> UpdateVoucherStatusByCode(Voucher voucher)
        {
            var rowAffected = 0;
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_UpdateVoucherStatusByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherStatus", voucher.VoucherStatus);

                return await conn.ExecuteAsync("usp_UpdateVoucherStatusByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> DeleteVoucherByCode(string code)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_DeleteVoucherByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", code);

                return await conn.ExecuteAsync("usp_DeleteVoucherByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> DeleteVoucherById(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_DeleteVoucherById"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherId", voucher.Id);

                return await conn.ExecuteAsync("usp_DeleteVoucherByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        // public static implicit operator BaseRepository(GiftVoucher v)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
