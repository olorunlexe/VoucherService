using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
            get { return new SqlConnection(_config.GetConnectionString("ConnectionString")); }
        }


       
        // public IEnumerable<Voucher> GetAllVouchers()
        // {
        //     using (var conn = Connection)
        //     {
        //         if (conn.State == ConnectionState.Closed)
        //             conn.Open();

        //         return conn.Query<Voucher>("usp_GetAllVouchers", commandType: CommandType.StoredProcedure).ToList();
        //     }
        // }

<<<<<<< HEAD
        public IEnumerable<Voucher> GetAllVouchers(Voucher voucher)
=======
        public IEnumerable<Voucher> GetAllVouchersFilterByMerchantId(string merchantId)
>>>>>>> 57b5f2e70cb83b909bc8d274a6258f7757e9e1cf
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);
                return conn.Query<Voucher>("usp_GetAllVouchersFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

<<<<<<< HEAD
        public Voucher GetVoucherByCode(Voucher voucher)
=======
        public Voucher GetVoucherByCode(string code)
>>>>>>> 57b5f2e70cb83b909bc8d274a6258f7757e9e1cf
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

               DynamicParameters parameters = new DynamicParameters();
                
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return conn.QuerySingle("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher GetVoucherByCodeAndType(Voucher voucher)
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
                return conn.QuerySingle("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Voucher GetVoucherByCreationDate(Voucher voucher)
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

        public void DeleteVoucherByCode(string code)
        {
            var rowAffected = 0;
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_DeleteVoucherByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", code);

                rowAffected = conn.Execute("usp_DeleteVoucherByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        // public static implicit operator BaseRepository(GiftVoucher v)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
