using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.DiscountVoucher
{
    class DiscountRepositoryImpl : BaseService,IDiscountRepository
    {
        public DiscountRepositoryImpl(IConfiguration config) : base(config)
        {
        }

    
    }

}
