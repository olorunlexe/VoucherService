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


        public async Task<Voucher> GetVoucherByCode(string code)
        {
            string encryptedCode = CodeGenerator.Encrypt(code);
            Voucher voucher =await _baseRepository.GetVoucherByCodeAsync(encryptedCode);
            string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
            voucher.Code = decryptedCode;
            return voucher;
        }

        /// <summary>
        /// Returns all vouchers created by a merchant regardless of the their type
        /// </summary>
        /// <param name="merchantId">the id of the merchant that created the vouchers</param>
        /// <returns>a list of vouchers</returns>
        public async Task<IEnumerable<Voucher>> GetAllVouchers(string merchantId)
        {
            var vouchers = await _baseRepository.GetAllVouchersFilterByMerchantIdAsync(merchantId);
            foreach(var voucher in vouchers)
            {
                string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                voucher.Code = decryptedCode;
            }
            return vouchers;
        }

        public Task DeleteVoucher(string code)
        {
            try
            {
                string encryptedCode = CodeGenerator.Encrypt(code);
                return _baseRepository.DeleteVoucherByCodeAsync(encryptedCode);
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
                string encryptedCode = CodeGenerator.Encrypt(code);
                var voucher = await GetVoucherByCode(encryptedCode);
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
                string encryptedCode = CodeGenerator.Encrypt(code);
                var voucher = await GetVoucherByCode(encryptedCode);
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
            //try
            //{
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await GetVoucherByCode(encryptedCode);
            Gift giftVoucher = await _giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
                giftVoucher.GiftBalance = amount; // do the update
                await _giftVoucherService.UpdateGiftVoucherBalance(giftVoucher); //persist the change   
                                                                                 //get the full gift voucher:TODO, I really wish we could avoid this

                return voucher;
            //}

            //catch (VoucherUpdateException ex)
            //{
            //    _logger.LogError(ex, "Could not perform update operation on voucher with {Code}", code);
            //    return null;
            //}
        }

        public async Task<long?> UpdateVoucherExpiryDate(string code, DateTime newDate)
        {
            try
            {
                //get the voucher that is to be updated
                string encryptedCode = CodeGenerator.Encrypt(code);
                var voucher = await GetVoucherByCode(encryptedCode);
                voucher.ExpiryDate = newDate;
                return await _baseRepository.UpdateVoucherExpiryDateByCodeAsync(voucher); //TODO: look into this:::await this                
            }
            catch (VoucherUpdateException ex)
            {
                _logger.LogError(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId)
        {
            try
            {
                var vouchers= await _giftVoucherService.GetAllGiftVouchers(merchantId);
                foreach (var voucher in vouchers)
                {
                    string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                    voucher.Code = decryptedCode;
                }
                return vouchers;
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
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await GetVoucherByCode(encryptedCode);
            Gift voucherResponse= await _giftVoucherService.GetGiftVoucher(voucher);
            string decryptedCode = CodeGenerator.Decrypt(voucherResponse.Code);
            voucherResponse.Code = decryptedCode;
            return voucherResponse;
        }

        public async Task<Value> GetValueVoucher(string code)
        {
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await GetVoucherByCode(encryptedCode);
            Value voucherResponse= await _valueVoucherService.GetValueVoucher(voucher);
            string decryptedCode = CodeGenerator.Decrypt(voucherResponse.Code);
            voucherResponse.Code = decryptedCode;
            return voucherResponse;
        }

        public async Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId)
        {
            var vouchers=await _valueVoucherService.GetAllValueVouchers(merchantId);
            foreach (var voucher in vouchers)
            {
                string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                voucher.Code = decryptedCode;
            }
            return vouchers;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountVouchers(string merchantId)
        {
            var vouchers=await _discountVoucherService.GetAllDiscountVouchersFilterByMerchantId(merchantId);
            foreach (var voucher in vouchers)
            {
                string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                voucher.Code = decryptedCode;
            }
            return vouchers;
        }

        public async Task<Discount> GetDiscountVoucher(string code)
        {
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await GetVoucherByCode(encryptedCode);
            Discount voucherResponse= await _discountVoucherService.GetDiscountVoucher(voucher);
            string decryptedCode = CodeGenerator.Decrypt(voucherResponse.Code);
            voucherResponse.Code = decryptedCode;
            return voucherResponse;
        }

        public async Task UpdateRedemptionCount(string code)
        {
            string encryptedCode = CodeGenerator.Encrypt(code);
            var discount = await GetDiscountVoucher(encryptedCode);
            await _discountVoucherService.UpdateRedemptionCount(discount);
        }
    }

}
