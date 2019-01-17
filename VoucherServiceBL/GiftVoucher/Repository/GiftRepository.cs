using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;
using Dapper;
using System.Data.SqlClient;

namespace VoucherServiceBL.GiftVoucher.Repository
{
    public class GiftRepository :BaseRepository, IGiftRepository
    {
        SqlConnection Connection {get {return new SqlConnection();}}
        public Gift CreateGiftVoucher(Gift voucher)
        {
            using (var connection = Connection)
            {
                if (connection.State.ToString() == "Closed") {connection.Open();}
                string storedProcedure = "usp_CreateVoucher";
                
                // DynamicParameters params = new DynamicParameters();
                params.
                var affectedRows = connection.Execute()
            }
        }

        public IEnumerable<Gift> GetAllGiftVouchers(string merchantId)
        {
            throw new NotImplementedException();
        }
        public void UpdateGiftVoucherAmount(BigInteger id, Gift voucher)
        {
            throw new NotImplementedException();
        }
    }
}