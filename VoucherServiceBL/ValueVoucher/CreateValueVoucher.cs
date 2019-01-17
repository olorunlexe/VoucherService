using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.ValueVoucher
{
    class CreateValueVoucher : BaseService
    {
        public CreateValueVoucher(IConfiguration config) : base(config)
        {
        }
        public static async Task<string> CreateValueVoucherAsync(string sample)
        {

            try
            {
                int rowAffected = 0;
                using (var conn = Connection)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    //Parameters Declaration to be passed into Stored procdure "CreateDiscountVoucher"..
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@HashedCode", sample);
                    parameters.Add("@VoucherType", sample);
                    parameters.Add("@ExpiryDate", sample);
                    parameters.Add("@MercahntId", sample);
                    parameters.Add("@ValueAmount", sample);

                    rowAffected = await conn.ExecuteAsync("CreateValueVoucher", parameters, commandType: CommandType.StoredProcedure);
                }

                //response using predefined serviceresponse class
                string response = "Good Request";
                return response;
            }
            catch (Exception e)
            {
                string response = null;
                return response;
            }

        }


    }
}
