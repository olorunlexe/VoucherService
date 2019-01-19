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

        public BaseService(IGiftVoucher giftService, IDiscountVoucher discountService, BaseRepository baseRepository)
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

        /// <summary>
        /// Returns all vouchers created by a merchant regardless of the their type
        /// </summary>
        /// <param name="merchantId">the id of the merchant that created the vouchers</param>
        /// <returns>a list of vouchers</returns>
        public IEnumerable<Voucher> GetAllVouchers(string merchantId) 
        {
            return baseRepository.GetAllVouchersFilterByMerchantId(merchantId);
        }

        /// <summary>
        /// Update the expiry date, status of a voucher or deactivate the voucher
        /// If the voucher is a gift voucher optionally update the gift amount
        /// </summary>
        /// <param name="voucherUpdateReq">object containing the voucher properties to change</param>
        /// <returns>Updated voucher</returns>
        public Voucher UpdateVoucher(VoucherUpdateReq voucherUpdateReq)
        {
            //get the voucher that is to be updated
            var voucher = GetVoucherByCode(voucherUpdateReq.Code);
            
            //update the fields that needs to be updated
            if(voucherUpdateReq.ExpiryDate != null) //should update expiry date of voucher
            {    
                voucher.ExpiryDate = voucherUpdateReq.ExpiryDate;
                baseRepository.UpdateVoucherExpiryDateByCode(voucher);
            }
            if(!string.IsNullOrEmpty(voucherUpdateReq.Status)) //should update status of voucher
            {    
                voucher.VoucherStatus = voucherUpdateReq.Status;
                baseRepository.UpdateVoucherStatusByCode(voucher);
            }

            if(voucherUpdateReq.GiftAmount != null) 
            {
                //get the full gift voucher:TODO, I really wish we could avoid this
                Gift giftVoucher = giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
                giftVoucher.GiftAmount = voucherUpdateReq.GiftAmount; // do the update
                giftVoucherService.UpdateGiftVoucher(giftVoucher); //persist the change
            }
            return voucher;
        }

        public void DeleteVoucher(string code)
        {
            baseRepository.DeleteVoucherByCode(code);
        }

    }
}