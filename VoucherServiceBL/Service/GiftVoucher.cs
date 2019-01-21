using System.Collections.Generic;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public class GiftVoucher : IGiftVoucher
    {
        public IGiftRepository Repository ;
        public CodeGenerator CodeGenerator;

        public GiftVoucher(IGiftRepository repository)
        {
            this.Repository = repository;
        }
        
        public Gift CreateGiftVoucher(VoucherRequest giftRequest)
        {
            //create the gift object from the Vouher
            Gift giftVoucher;
            giftVoucher = new Gift() 
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
            return Repository.CreateGiftVoucher(giftVoucher);
        }

        public Gift GetGiftVoucher(Voucher voucher)
        {
            throw new System.NotImplementedException();
        }

        public Voucher UpdateGiftVoucher(Gift giftVoucher)
        {
            throw new System.NotImplementedException();
        }

    }


}