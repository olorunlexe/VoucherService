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

                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

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
        public void UpdateGiftVoucherAmount(BigInteger id, Gift voucher)
        {
            using (var connection = Connection)
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                var storedProcedure = "usp_UpdateGiftAmountById";
                var parameters = new DynamicParameters();
                parameters.Add("@VoucherId", id);
                parameters.Add("@GiftAmount", voucher.GiftAmount);

                connection.Execute(storedProcedure, parameters, commandType:CommandType.StoredProcedure);
            }
        }
    }
}