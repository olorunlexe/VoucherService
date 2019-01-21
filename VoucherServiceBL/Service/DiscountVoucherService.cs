using System;
using System.Collections.Generic;
using System.Text;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public class DiscountVoucherService : IDiscountVoucherService
    {
        public IDiscountRepository discountRepository;
        public CodeGenerator CodeGenerator;


        public DiscountVoucherService(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }


        public Discount CreateDiscountVoucher(VoucherRequest discountRequest)
        {
            //create the gift object from the Vouher
            Discount discountVoucher;
            discountVoucher = new Discount()
            {
                Code = CodeGenerator.HashedCode(discountRequest),
                CreationDate = discountRequest.CreationDate,
                ExpiryDate = discountRequest.ExpiryDate,
                VoucherStatus = "Active",
                VoucherType = discountRequest.VoucherType,
                Description = discountRequest.Description,
                DiscountAmount = discountRequest.DiscountAmount,
                DiscountUnit = discountRequest.DiscountUnit,
                DiscountPercent = discountRequest.DiscountPercent,
                RedemptionCount = 0L,
                MerchantId = discountRequest.MerchantId,
                Metadata = discountRequest.Metadata
            };
            //persist the object to the db    
            return discountRepository.CreateDiscountVoucher(discountVoucher);
        }

        public Discount GetDiscountVoucher(Voucher voucher)
        {
            return discountRepository.GetDiscountVoucher(voucher);
        }

        public IEnumerable<Discount> GetAllDiscountVouchersFilterByMerchantId(string merchantId)
        {
            return discountRepository.GetAllDiscountVouchersFilterByMerchantId(merchantId);
        }
    }
}
