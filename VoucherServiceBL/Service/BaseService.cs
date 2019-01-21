using System.Collections.Generic;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public class BaseService:IVoucherService
    {

        private IGiftVoucher giftVoucherService;
        private IDiscountVoucher discountVoucherService;
        private BaseRepository baseRepository;

        public BaseService(IGiftVoucher giftService, IDiscountVoucher discountService)
        {
            this.giftVoucherService = giftService;
            this.discountVoucherService = discountService;
            var repository = (giftService as GiftVoucher).Repository;
            var giftRepository =  repository as GiftRepository;
            this.baseRepository = giftRepository as BaseRepository;
        }

        public Voucher CreateVoucher(VoucherRequest voucherRequest)
        {
            //let each voucher service handle its own creation

            if (voucherRequest.VoucherType.ToUpper() == "GIFT" ) 
                return giftVoucherService.CreateGiftVoucher(voucherRequest);
            if (voucherRequest.VoucherType.ToUpper() == "DISCOUNT") 
                return discountVoucherService.CreateDiscountVoucher(voucherRequest);

            return null; //TODO: remove this
        }

        
        public Voucher GetVoucherByCode(string code)
        {
            return baseRepository.GetVoucherByCode(code);
        }

        // public Voucher GetVoucherByExpiryDate(Voucher voucher)
        // {
            
        // }
        public IEnumerable<Voucher> GetAllVouchers(string merchantId) 
        {
            return baseRepository.GetAllVouchersFilterByMerchantId(merchantId);
        }

        public Voucher UpdateVoucherExpiryDate(Voucher voucher)
        {
            return baseRepository.UpdateVoucherExpiryDateByCode(voucher);
        }

        public IDiscountVoucher  GetDiscountVoucher()
        {
            return this.discountVoucherService;
        }
        public Voucher UpdateVoucher(VoucherUpdateReq voucherUpdateReq)
        {
            //I need a logic to combine repo.UpdateStatus and repo.UpdateExpiryDate
            //get the voucher that is to be updated
            var voucher = GetVoucherByCode(voucherUpdateReq.Code);
            //update the fields that needs to be updated
            if(voucherUpdateReq.ExpiryDate != null) 
            {    
                voucher.ExpiryDate = voucherUpdateReq.ExpiryDate;
                baseRepository.UpdateVoucherExpiryDateByCode(voucher);
            }
            if(!string.IsNullOrEmpty(voucherUpdateReq.Status)) 
            {    
                voucher.VoucherStatus = voucherUpdateReq.Status;
                baseRepository.UpdateVoucherStatusByCode(voucher);
            }

            if(voucherUpdateReq.GiftAmount != null) 
            {
                //get the full gift voucher:TODO, I really wish we could avoid this
                Gift giftVoucher = giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
                giftVoucher.GiftAmount = voucherUpdateReq.GiftAmount; // do the update
                giftVoucherService.UpdateGiftVoucher(giftVoucher);
            }
            return voucher;
        }

        public void DeleteVoucher(string code)
        {
            baseRepository.DeleteVoucherByCode(code);
        }


        //TODO: implement this method
        public Voucher UpdateVoucher(string voucher)
        {
            throw new System.NotImplementedException();
        }

        // public Voucher UpdateVoucherStatusByCode, Expry(Voucher voucher)

    }
}