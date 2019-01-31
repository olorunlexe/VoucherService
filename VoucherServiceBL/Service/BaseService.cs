using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Exceptions;
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

        public async Task<int> CreateVoucher(VoucherRequest voucherRequest)
        {
            var numOfVouchersCreated = 0;

            //let each voucher service handle its own creation
            // foreach (var num in Enumerable.Range(1, voucherRequest.NumbersOfVoucherToCreate))
            // {
                try
                {
                    if (voucherRequest.VoucherType.ToUpper() == "GIFT" )
                    {
                        numOfVouchersCreated += await giftVoucherService.CreateGiftVoucher(voucherRequest);
                    } 
                        
                    else if (voucherRequest.VoucherType.ToUpper() == "DISCOUNT") 
                    {
                        numOfVouchersCreated += await discountVoucherService.CreateDiscountVoucher(voucherRequest);
                    }
                        
                    else 
                    {
                        numOfVouchersCreated += await valueVoucherService.CreateValueVoucher(voucherRequest);    
                        
                    }

                    //TODO: Log the event (Voucher Created) 
                        
                }
                catch (VoucherCreateException  ex) //TODO: something happened handle it                //persist the object to the db    
                //if some error occurred and not all voucher could be created log the error
                {
                    //TODO: Log the error
                    //handle the error here; what should happen, try again or what
                    throw new VoucherCreateException
                            ($"An error occurred. {numOfVouchersCreated} Vouchers already created. Voucher number could be created.");
                }
            // }

            return numOfVouchersCreated;
            
        }

        
        public Task<Voucher> GetVoucherByCode(string code)
        {
            return baseRepository.GetVoucherByCode(code);
        }

        /// <summary>
        /// Returns all vouchers created by a merchant regardless of the their type
        /// </summary>
        /// <param name="merchantId">the id of the merchant that created the vouchers</param>
        /// <returns>a list of vouchers</returns>
        public Task<IEnumerable<Voucher>> GetAllVouchers(string merchantId) 
        {
            return baseRepository.GetAllVouchersFilterByMerchantId(merchantId);
        }

        public Task DeleteVoucher(string code)
        {
            try
            {
                return baseRepository.DeleteVoucherByCode(code);                
            }
            catch (SqlException)
            {
                
                throw;
            }
        }

        public async Task<int> ActivateOrDeactivateVoucher(string code)
        {
            try
            {
                var voucher = await GetVoucherByCode(code);
                voucher.VoucherStatus = voucher.VoucherStatus== "ACTIVE" ? "INACTIVE" : "ACTIVE";
                return await baseRepository.UpdateVoucherStatusByCode(voucher);                
            }
            catch (SqlException ex)
            {
                throw;
            }
            //get the voucher that is to be updated
        }

        public async Task<Voucher> UpdateGiftVoucherAmount(string code, long amount)
        {
            try
            {
                var voucher = await GetVoucherByCode(code);
                Gift giftVoucher = await giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
                giftVoucher.GiftAmount = amount; // do the update
                await giftVoucherService.UpdateGiftVoucher(giftVoucher); //persist the change   
                //get the full gift voucher:TODO, I really wish we could avoid this
    
                return voucher;
            }              
            
            catch (VoucherUpdateException ex)
            {
                
                throw;
            }

        }

        public async Task<int> UpdateVoucherExpiryDate(string code, DateTime newDate)
        {
            try
            {
                //get the voucher that is to be updated
                var voucher = await GetVoucherByCode(code);
                voucher.ExpiryDate = newDate;
                return await baseRepository.UpdateVoucherExpiryDateByCode(voucher); //TODO: look into this:::await this                
            }
            catch (VoucherUpdateException ex)
            {
                throw;
            }

        }

        public Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId)
        {
            try
            {
               return  giftVoucherService.GetAllGiftVouchers(merchantId);            
            }
            catch (System.Exception ex)
            {
                
                throw;
            }
        }

        public async Task<Gift> GetGiftVoucher(string code)
        {
            var voucher = await GetVoucherByCode(code);
            return await giftVoucherService.GetGiftVoucher(voucher);
        }

        public async Task<Value> GetValueVoucher(string code)
        {
            try
            {
                var voucher = await GetVoucherByCode(code);
                return await valueVoucherService.GetValueVoucher(voucher);                
            }
            catch (SqlException ex)
            {                
                throw;
            }

        }

        public Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId)
        {
            return valueVoucherService.GetAllValueVouchers(merchantId);
        }

        public Task<IEnumerable<Discount>> GetAllDiscountVouchers(string merchantId)
        {
            try 
            {
                return discountVoucherService.GetAllDiscountVouchersFilterByMerchantId(merchantId);
            } 
            catch (SqlException ex) 
            {
                //TODO: log the error
                throw; //FIXME: handle this better
            }
        }

        public async Task<Discount> GetDiscountVoucher(string code)
        {
            try
            {
                var voucher = await GetVoucherByCode(code);
                return await discountVoucherService.GetDiscountVoucher(voucher);                
            }
            catch (SqlException ex)
            {
                //TODO: log the error
                throw;
            }

        }
    }
}
