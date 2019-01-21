using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace VoucherServiceBL.Repository
{
    public class GiftRepository :BaseRepository, IGiftRepository
    {
        public GiftRepository(IConfiguration configuration):base(configuration) {}
        public Gift CreateGiftVoucher(Gift voucher)
        {
            using (var connection = Connection)
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                string storedProcedure = "usp_CreateGiftVoucher";
                var parameters = new DynamicParameters();
                parameters.Add("@HashedCode", voucher.Code);
                parameters.Add("@ExpiryDate", voucher.ExpiryDate);
                parameters.Add("@MerchantId", voucher.MerchantId);
                parameters.Add("@GiftAmount", voucher.GiftAmount);

                var result=connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                voucher.Id = result;
                return voucher;
            }
        }

        public IEnumerable<Gift> GetAllGiftVouchers(string merchantId)
        {
            using (var connection = Connection)
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                var storedProcedure = "usp_GetAllGiftVouchersFilterByMerchantId";
                var parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);

                var res = connection.QueryMultiple(storedProcedure, parameters, commandType:CommandType.StoredProcedure);
                return res.Read<Gift>().AsList();
            }
        }

        public Gift GetGiftVoucher(Voucher voucher)
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
                return conn.QuerySingle<Gift>("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public Voucher UpdateGiftVoucherAmount(string code, Gift voucher)
        {
            using (var connection = Connection)
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                var storedProcedure = "usp_UpdateGiftAmountByCode";
                var parameters = new DynamicParameters();
                parameters.Add("@Code", code);
                parameters.Add("@GiftAmount", voucher.GiftAmount);
                connection.Execute(storedProcedure, parameters, commandType:CommandType.StoredProcedure);
            }
            return voucher;
        }
        
    }
}