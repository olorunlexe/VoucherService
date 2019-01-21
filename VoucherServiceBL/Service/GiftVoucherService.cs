using System.Collections.Generic;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public class GiftVoucherService : IGiftVoucherService
    {
        private IGiftRepository repository ;

        public IGiftRepository GiftRepository => this.repository;

        public GiftVoucherService(IGiftRepository repository)
        {
            this.repository = repository;
        }


        public Gift CreateGiftVoucher(VoucherRequest giftRequest)
        {
            //create the gift object from the Vouher
            Gift giftVoucher = new Gift() 
                {Code = CodeGenerator.HashedCode(giftRequest),
                 CreationDate = giftRequest.CreationDate,
                 ExpiryDate = giftRequest.ExpiryDate,
                 VoucherStatus = "Active",
                 VoucherType = giftRequest.VoucherType,
                 Description = giftRequest.Description,
                 GiftAmount = giftRequest.GiftAmount,
                 MerchantId = giftRequest.MerchantId,
                 Metadata = giftRequest.Metadata,
                 GiftBalance = giftRequest.GiftAmount //giftbalance == gift amount at creation
                };

            //persist the object to the db    
            return GiftRepository.CreateGiftVoucher(giftVoucher);
        }

        public Gift GetGiftVoucher(Voucher voucher)
        {
            return GiftRepository.GetGiftVoucher(voucher);
        }

        public IEnumerable<Gift> GetAllGiftVouchers(string merchantId)
        {
            return GiftRepository.GetAllGiftVouchers(merchantId);
        }

        public Voucher UpdateGiftVoucher(Gift giftVoucher)
        {
            throw new System.NotImplementedException();
        }   

    }
}