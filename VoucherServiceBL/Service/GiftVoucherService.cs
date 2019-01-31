using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Exceptions;
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


        public  Task<int> CreateGiftVoucher(VoucherRequest giftRequest)
        {
            // var numOfVouchersCreated = 0;
            
            var vouchersList = new List<Gift>(giftRequest.NumbersOfVoucherToCreate);
            
            //create the gift object from the Vouher
            foreach (var num in Enumerable.Range(1, giftRequest.NumbersOfVoucherToCreate))
                {
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
            
                vouchersList.Add(giftVoucher);
            }
            return GiftRepository.CreateGiftVoucher(vouchersList);
        }

        public Task<Gift> GetGiftVoucher(Voucher voucher)
        {
            return GiftRepository.GetGiftVoucher(voucher);
        }

        public Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId)
        {
            return GiftRepository.GetAllGiftVouchers(merchantId);
        }

        public async Task<int> UpdateGiftVoucher(Gift giftVoucher)
        {
            var numOfVouchersUpdated = await GiftRepository.UpdateGiftVoucherAmount(giftVoucher);
            if (numOfVouchersUpdated == 0)
            {
                throw new VoucherUpdateException("Error occurred. Could not update voucher");
            }
            return numOfVouchersUpdated;
        }   

    }
}