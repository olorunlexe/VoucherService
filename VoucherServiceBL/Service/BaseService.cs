using System;
using System.Collections.Generic;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public class BaseService:IVoucherService
    {

        private IGiftVoucherService giftVoucherService;
        private IDiscountVoucherService discountVoucherService;
        private IValueVoucherService valueVoucherService;
        private BaseRepository baseRepository;

        //inject the services
        public BaseService(IGiftVoucherService giftService, IDiscountVoucherService discountService,
                           IValueVoucherService valueService)
        {
            this.giftVoucherService = giftService;
            this.discountVoucherService = discountService;
            this.valueVoucherService = valueService;
            this.baseRepository = GetBaseRepository();
        }

        private BaseRepository GetBaseRepository()
        {
            IGiftRepository giftRepository =  giftVoucherService.GiftRepository as GiftRepository;
            return giftRepository as BaseRepository;
        }

        public Voucher CreateVoucher(VoucherRequest voucherRequest)
        {
            //let each voucher service handle its own creation

            if (voucherRequest.VoucherType.ToUpper() == "GIFT" ) 
                return giftVoucherService.CreateGiftVoucher(voucherRequest);
            else if (voucherRequest.VoucherType.ToUpper() == "DISCOUNT") 
                return discountVoucherService.CreateDiscountVoucher(voucherRequest);
            else 
                return valueVoucherService.CreateValueVoucher(voucherRequest);
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

        public void DeleteVoucher(string code)
        {
            baseRepository.DeleteVoucherByCode(code);
        }

        public Voucher ActivateOrDeactivateVoucher(string code)
        {
            //get the voucher that is to be updated
            var voucher = GetVoucherByCode(code);
            voucher.VoucherStatus = voucher.status == "ACTIVE" ? "INACTIVE" : "ACTIVE";
            return  baseRepository.UpdateVoucherStatusByCode(voucher);
            
        }

        public Voucher UpdateGiftVoucherAmount(string code, long amount)
        {
            var voucher = GetVoucherByCode(code);

            //get the full gift voucher:TODO, I really wish we could avoid this
            Gift giftVoucher = giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
            giftVoucher.GiftAmount = amount; // do the update
            return giftVoucherService.UpdateGiftVoucher(giftVoucher); //persist the change
        }

        public Voucher UpdateVoucherExpiryDate(string code, DateTime newDate)
        {
            //get the voucher that is to be updated
            var voucher = GetVoucherByCode(code);
            voucher.ExpiryDate = newDate;
            return baseRepository.UpdateVoucherExpiryDateByCode(voucher);
        }

        public IEnumerable<Gift> GetAllGiftVouchers(string merchantId)
        {
           return giftVoucherService.GetAllGiftVouchers(merchantId);
        }

        public Gift GetGiftVoucher(string code)
        {
            var voucher = GetVoucherByCode(code);
            return giftVoucherService.GetGiftVoucher(voucher);
        }

        public Value GetValueVoucher(string code)
        {
            var voucher = GetVoucherByCode(code);
            return valueVoucherService.GetValueVoucher(voucher);
        }

        public IEnumerable<Value> GetAllValueVouchers(string merchantId)
        {
            return valueVoucherService.GetAllValueVouchers(merchantId);
        }

        public IEnumerable<Discount> GetAllDiscountVouchers(string merchantId)
        {
            return discountVoucherService.GetAllDiscountVouchersFilterByMerchantId(merchantId);
        }

        public Discount GetDiscountVoucher(string code)
        {
            var voucher = GetVoucherByCode(code);
            return discountVoucherService.GetDiscountVoucher(voucher);
        }
    }
}