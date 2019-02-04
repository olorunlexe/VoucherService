using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllVouchersFilterByMerchantId(string merchantId);

        Task<Voucher> GetVoucherByCode(string code);

         Task<Voucher> GetVoucherByCodeFilterByMerchantId(Voucher voucher);

        Task<Voucher> GetVoucherByCreationDate(Voucher voucher);

        Task<Voucher> GetVoucherByCreationDateFilterByMerchantId(Voucher voucher);

        Task<Voucher> GetVoucherByExpiryDate(Voucher voucher);

        Task<Voucher> GetVoucherByExpiryDateFilterByMerchantId(Voucher voucher);

        // Task<Voucher> GetVoucherById(Voucher voucher);

        // Task<Voucher> GetVoucherByIdFilterByMerchantId(Voucher voucher);

        Task<Voucher> GetVoucherByMerchantId(Voucher voucher);

        Task<Voucher> GetVoucherByStatus(Voucher voucher);

        Task<int> UpdateVoucherExpiryDateByCode(Voucher voucher);

        Task<int> UpdateVoucherStatusByCode(Voucher voucher);

        Task<int> DeleteVoucherByCode(string code);

        // Task<int> DeleteVoucherById(Voucher voucher);
    }
}
