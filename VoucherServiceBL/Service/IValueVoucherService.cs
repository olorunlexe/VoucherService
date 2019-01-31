using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.ValueVoucher.Repository;

namespace VoucherServiceBL.Service
{
    /// <summary>
    /// A interface that handles the management of a value voucher
    /// </summary>
    

    public interface IValueVoucherService
    {
        Task<int> CreateValueVoucher(VoucherRequest value);

        Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId);

        Task<Value> GetValueVoucher(Voucher voucher);
    }
}