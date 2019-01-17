using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL
{
    public class BaseRepository
    {
        private static IConfiguration _config;

        public BaseRepository(IConfiguration config) => _config = config;

        public static IDbConnection Connection
        {
            get { return new SqlConnection(_config.GetConnectionString("ConnectionString")); }
        }


       
        public List<Voucher> GetAllVouchers()
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return conn.Query<Voucher>("usp_GetAllVouchers", commandType: CommandType.StoredProcedure).ToList();
            }

        }

        public List<Voucher> GetAllVouchersFilterByMerchantId(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", voucher.MerchantId);
                return conn.Query<Voucher>("usp_GetAllVouchersFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Voucher GetVoucherByCode(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

               DynamicParameters parameters = new DynamicParameters();
              
                parameters.Add("@MerchantId", voucher.MerchantId);

                return conn.QuerySingle("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher GetVoucherByCodeFilterByMerchantId(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);
                return conn.QuerySingle("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        
        public Voucher GetVoucherByCreationDateFilterByMerchantId(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByCreationDate"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CreationDate", voucher.CreationDate);
                // parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return conn.QuerySingle("usp_GetVoucherByCreationDateFilterByMerchantId",parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher GetVoucherByExpiryDateFilterByMerchantId(Voucher voucher)
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

                return conn.QuerySingle("usp_GetVoucherByExpiryDateFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher GetVoucherByStatus(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByStatus"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", voucher.MerchantId);
                parameters.Add("@VoucherStatus", voucher.VoucherStatus);

                return conn.QuerySingle("usp_GetVoucherByStatusFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher UpdateVoucherExpiryDateByCode(Voucher voucher)
        {
            var rowAffected = 0;
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_UpdateVoucherExpiryDateByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add(" @ExpiryDate", voucher.ExpiryDate);

                rowAffected = conn.Execute("usp_UpdateVoucherExpiryDateByCode", parameters, commandType: CommandType.StoredProcedure);
            }
            return voucher;
        }

        public Voucher UpdateVoucherStatusByCode(Voucher voucher)
        {
            var rowAffected = 0;
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_UpdateVoucherStatusByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add(" @VoucherStatus", voucher.VoucherStatus);

                rowAffected = conn.Execute("usp_UpdateVoucherStatusByCode", parameters, commandType: CommandType.StoredProcedure);
            }
            return voucher;
        }

        public Voucher DeleteVoucherByCode(Voucher voucher)
        {
            var rowAffected = 0;
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_DeleteVoucherByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);

                rowAffected = conn.Execute("usp_DeleteVoucherByCode", parameters, commandType: CommandType.StoredProcedure);
            }
            return voucher;

        }


    }
}
