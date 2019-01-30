using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    public interface IDiscountRepository
    {
        Task<int> CreateDiscountVoucher(Discount discount);
        Task<Discount> GetDiscountVoucher(Voucher voucher);
        Task<IEnumerable<Discount>> GetAllDiscountVouchersFilterByMerchantId(string merchantId);

    }

}
