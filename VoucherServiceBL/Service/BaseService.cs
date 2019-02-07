using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VoucherServiceBL.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Exceptions;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;
using VoucherServiceBL.Repository.SqlServer;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace VoucherServiceBL.Service
{
    public class BaseService : IVoucherService
    {

        private IGiftVoucherService _giftVoucherService;
        private IDiscountVoucherService _discountVoucherService;
        private IValueVoucherService _valueVoucherService;
        private IVoucherRepository _baseRepository;
        private ILogger<BaseService> _logger;

        //inject the services
        public BaseService(IGiftVoucherService giftService, IDiscountVoucherService discountService,
                           IValueVoucherService valueService, IVoucherRepository voucherRepository, ILogger<BaseService> logger)
        {
            this._giftVoucherService = giftService;
            this._discountVoucherService = discountService;
            this._valueVoucherService = valueService;
            this._baseRepository = voucherRepository;
            this._logger = logger;
        }

        // private BaseRepository GetBaseRepository()
        // {
        //     IGiftRepository giftRepository =  _giftVoucherService.GiftRepository as GiftRepository;
        //     return giftRepository as BaseRepository;
        // }

        public async Task<int?> CreateVoucher(VoucherRequest voucherRequest)
        {
            var numOfVouchersCreated = 0;

            //let each voucher service handle its own creation
            try
            {
                voucherRequest.CreationDate = DateTime.Now;

                if (voucherRequest.VoucherType.ToUpper() == "GIFT")
                {
                    numOfVouchersCreated += await _giftVoucherService.CreateGiftVoucher(voucherRequest);
                }

                else if (voucherRequest.VoucherType.ToUpper() == "DISCOUNT")
                {
                    numOfVouchersCreated += await _discountVoucherService.CreateDiscountVoucher(voucherRequest);
                }

                else
                {
                    numOfVouchersCreated += await _valueVoucherService.CreateValueVoucher(voucherRequest);

                }

                //TODO: Log the event (Voucher Created) 
                _logger.LogInformation("Created {Number}: vouchers for {Merchant}",
                        numOfVouchersCreated, voucherRequest.MerchantId);

                return numOfVouchersCreated;
            }
            catch (VoucherCreateException ex) //TODO: something happened handle it
                                              //if some error occurred and not all voucher could be created log the error
            {
                //TODO: Log the error
                _logger.LogError(ex, "An error occured while creating vouchers for {Merchant}", voucherRequest.MerchantId);
                //handle the error here; what should happen, try again or what
                return null;
            }
        }


        public Task<Voucher> GetVoucherByCode(string code)
        {
            return _baseRepository.GetVoucherByCodeAsync(code);
        }

        /// <summary>
        /// Returns all vouchers created by a merchant regardless of the their type
        /// </summary>
        /// <param name="merchantId">the id of the merchant that created the vouchers</param>
        /// <returns>a list of vouchers</returns>
        public Task<IEnumerable<Voucher>> GetAllVouchers(string merchantId)
        {
            return _baseRepository.GetAllVouchersFilterByMerchantIdAsync(merchantId);
        }

        public Task DeleteVoucher(string code)
        {
            try
            {
                return _baseRepository.DeleteVoucherByCodeAsync(code);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Could not perform delete operation on voucher with {Code}", code);
                return null;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Could not perform delete operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<long?> ActivateOrDeactivateVoucher(string code)
        {
            try
            {
                //get the voucher that is to be updated
                var voucher = await GetVoucherByCode(code);
                //voucher.VoucherStatus = voucher.VoucherStatus== "ACTIVE" ? "INACTIVE" : "ACTIVE";
                switch (voucher.VoucherStatus.ToUpper())
                {
                    case "ACTIVE": voucher.VoucherStatus = "INACTIVE"; break;
                    case "INACTIVE": voucher.VoucherStatus = "ACTIVE"; break;
                    default: voucher.VoucherStatus = "ACTIVE"; break;
                }
                return await _baseRepository.UpdateVoucherStatusByCodeAsync(voucher);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Could not perform activate or deactivate operation on voucher with {Code}", code);
                return null;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Could not perform activate or deactivate operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<Voucher> UpdateGiftVoucherAmount(string code, long amount)
        {
            try
            {
                var voucher = await GetVoucherByCode(code);
                Gift giftVoucher = await _giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
                giftVoucher.GiftAmount = amount; // do the update
                await _giftVoucherService.UpdateGiftVoucher(giftVoucher); //persist the change   
                                                                          //get the full gift voucher:TODO, I really wish we could avoid this

                return voucher;
            }

            catch (VoucherUpdateException ex)
            {
                _logger.LogError(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<Voucher> UpdateGiftVoucherBalance(string code, long amount)
        {
            try
            {
                var voucher = await GetVoucherByCode(code);
                Gift giftVoucher = await _giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
                giftVoucher.GiftAmount = amount; // do the update
                await _giftVoucherService.UpdateGiftVoucherBalance(giftVoucher); //persist the change   
                                                                                 //get the full gift voucher:TODO, I really wish we could avoid this

                return voucher;
            }

            catch (VoucherUpdateException ex)
            {
                _logger.LogError(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<long?> UpdateVoucherExpiryDate(string code, DateTime newDate)
        {
            try
            {
                //get the voucher that is to be updated
                var voucher = await GetVoucherByCode(code);
                voucher.ExpiryDate = newDate;
                return await _baseRepository.UpdateVoucherExpiryDateByCodeAsync(voucher); //TODO: look into this:::await this                
            }
            catch (VoucherUpdateException ex)
            {
                _logger.LogError(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }
        }

        public Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId)
        {
            try
            {
                return _giftVoucherService.GetAllGiftVouchers(merchantId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Could not perform retrieve operation on vouchers of {Merchant}", merchantId);
                return null;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Could not perform retrieve operation on vouchers of {Merchant}", merchantId);
                return null;
            }
        }

        public async Task<Gift> GetGiftVoucher(string code)
        {
            var voucher = await GetVoucherByCode(code);
            return await _giftVoucherService.GetGiftVoucher(voucher);
        }

        public async Task<Value> GetValueVoucher(string code)
        {
            var voucher = await GetVoucherByCode(code);
            return await _valueVoucherService.GetValueVoucher(voucher);
        }

        public Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId)
        {
            return _valueVoucherService.GetAllValueVouchers(merchantId);
        }

        public Task<IEnumerable<Discount>> GetAllDiscountVouchers(string merchantId)
        {
            return _discountVoucherService.GetAllDiscountVouchersFilterByMerchantId(merchantId);
        }

        public async Task<Discount> GetDiscountVoucher(string code)
        {
            var voucher = await GetVoucherByCode(code);
            return await _discountVoucherService.GetDiscountVoucher(voucher);
        }

        public async Task UpdateRedemptionCount(string code)
        {
            var discount = await GetDiscountVoucher(code);
            await _discountVoucherService.UpdateRedemptionCount(discount);
        }
    }

}
