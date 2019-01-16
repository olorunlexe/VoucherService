using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace VoucherServiceBL.Util
{
    class BaseService
    {
        private static IConfiguration _config;

        public BaseService(IConfiguration config) => _config = config;

        public static IDbConnection Connection
        {
            ////////
            get { return new SqlConnection(_config.GetConnectionString("ConnectionString")); }
        }
    }
}
