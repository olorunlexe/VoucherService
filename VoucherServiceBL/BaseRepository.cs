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


       
        public IEnumerable<Voucher> GetAllVouchers()
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return conn.Query<Voucher>("usp_GetAllVouchers", commandType: CommandType.StoredProcedure).ToList();
            }

        }

        public IEnumerable<Voucher> GetAllVouchersFilterByMerchantId(Voucher voucher)
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

        public Voucher GetVoucherByCode()
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return conn.QuerySingle("usp_GetVoucherByCode", commandType: CommandType.StoredProcedure);
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

        public Voucher GetVoucherByCreationDate(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
        //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByCreationDate"..
        DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CreationDate", voucher.CreationDate);
                parameters.Add("@VoucherType", voucher.VoucherType);
                return conn.QuerySingle("usp_GetVoucherByCreationDate",parameters, commandType: CommandType.StoredProcedure);
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
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return conn.QuerySingle("usp_GetVoucherByCreationDateFilterByMerchantId",parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher GetVoucherByExpiryDate(Voucher voucher)
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

                return conn.QuerySingle("usp_GetVoucherByExpiryDate", parameters, commandType: CommandType.StoredProcedure);
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
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return conn.QuerySingle("usp_GetVoucherByExpiryDateFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher GetVoucherById(Voucher voucher)
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

                return conn.QuerySingle("usp_GetVoucherById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher GetVoucherByIdFilterByMerchantId(Voucher voucher)
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

                return conn.QuerySingle("usp_GetVoucherByIdFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public Voucher GetVoucherByMerchantId(Voucher voucher)
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

                return conn.QuerySingle("usp_GetVoucherByMerchantId", parameters, commandType: CommandType.StoredProcedure);
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
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@VoucherStatus", voucher.VoucherStatus);

                return conn.QuerySingle("usp_GetVoucherByStatus", parameters, commandType: CommandType.StoredProcedure);
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

        public Voucher DeleteVoucherById(Voucher voucher)
        {
            var rowAffected = 0;
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_DeleteVoucherById"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherId", voucher.Id);

                rowAffected = conn.Execute("usp_DeleteVoucherByCode", parameters, commandType: CommandType.StoredProcedure);
            }
            return voucher;
        }

    }
}
