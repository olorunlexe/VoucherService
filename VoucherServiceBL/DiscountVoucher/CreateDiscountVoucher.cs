using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.DiscountVoucher
{
    class CreateDiscountVoucher : BaseService
    {
        public CreateDiscountVoucher(IConfiguration config) : base(config)
        {
        }
        public static async Task<string> CreateDiscountVoucherAsync(string sample)
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
                    parameters.Add("@Code", sample);
                    parameters.Add("@VoucherType", sample);
                    parameters.Add("@DiscountType", sample);

                    rowAffected = await conn.ExecuteAsync("CreateDiscountVoucher", parameters, commandType: CommandType.StoredProcedure);
                }

                //response using predefined serviceresponse class
                string response ="Good Request";
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
