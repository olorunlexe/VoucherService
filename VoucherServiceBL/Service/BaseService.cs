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
using VoucherServiceBL.Events;
using Serilog;

namespace VoucherServiceBL.Service
{
    public class BaseService:IVoucherService
    {

        private IGiftVoucherService _giftVoucherService;
        private IDiscountVoucherService _discountVoucherService;
        private IValueVoucherService _valueVoucherService;
        private IVoucherRepository _baseRepository;
        private ILogger<BaseService> _logger;

        //inject the services
        public BaseService(IGiftVoucherService giftService, IDiscountVoucherService discountService,
                           IValueVoucherService valueService, IVoucherRepository voucherRepository,ILogger<BaseService> logger)
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
                    voucherRequest.Metadata = Guid.NewGuid().ToString();
                    
                    if (voucherRequest.VoucherType.ToUpper() == "GIFT" )
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

                    //TODO: Log the event (VoucherCreated) 
                    var voucherGeneratedEvent = new VoucherGeneratedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucherRequest.MerchantId,
                        NumberGenerated = numOfVouchersCreated, Message = "New Vouchers created"
                    };

                    _logger.LogInformation("Created {Number}: vouchers for {Merchant} :{@Event}", 
                            numOfVouchersCreated, voucherRequest.MerchantId, voucherGeneratedEvent);
        
                    return numOfVouchersCreated;                
                }
                catch (VoucherCreateException ex) //TODO: something happened handle it
                //if some error occurred and not all voucher could be created log the error
                {
                    //TODO: Log the error event (VoucherGenerationFailed)
                    var generationFailedEvent = new VoucherGenerationFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucherRequest.MerchantId,
                        NumberToGenerate = voucherRequest.NumbersOfVoucherToCreate, Message = "Could not generate the vouchers",
                        FailureReason = ex.Message
                    };

                    _logger.LogError("Failed to Generate vouchers: {@Event}", generationFailedEvent);
                    _logger.LogDebug(ex, "An error occured while creating vouchers for {Merchant}", voucherRequest.MerchantId);
                    
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
                Task deleteTask = _baseRepository.DeleteVoucherByCodeAsync(code);
                var deleteEvent = new VoucherDeletedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now,
                        Message = "Deleted voucher", VoucherCode = code                    
                };

                _logger.LogInformation("Deleted Voucher: {@DeleteEvent}", deleteEvent);
                return deleteTask;                
            }

            catch (Exception ex) 
            {
                var deleteFailedEvent = new VoucherDeletionFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now,
                        Message = "Could not perform delete on voucher", VoucherCode = code,
                        FailureReason = ex.Message 
                };

                _logger.LogError("Deletion Failed on voucher: {@DeleteFailedEvent}", deleteFailedEvent);
                
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
                voucher.VoucherStatus = voucher.VoucherStatus== "ACTIVE" ? "INACTIVE" : "ACTIVE";


                long recordsAffected = await _baseRepository.UpdateVoucherStatusByCodeAsync(voucher);

                //log the event
                if (voucher.VoucherStatus == "ACTIVE")
                {
                    var updatedEvent = new VoucherDeactivatedEvent() {
                            EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucher.MerchantId,
                            Message = "Voucher was deactivated", VoucherCode = voucher.Code, 
                            VoucherType = voucher.VoucherType
                    };
                    _logger.LogInformation("Deactivated a voucher: {@Event}", updatedEvent);    
                }

                if (voucher.VoucherStatus == "INACTIVE")
                {
                    var updatedEvent = new VoucherReactivatedEvent() {
                            EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucher.MerchantId,
                            Message = "Voucher was Activated", VoucherCode = voucher.Code, 
                            VoucherType = voucher.VoucherType
                    };
                    _logger.LogInformation("Activated a voucher: {@Event}", updatedEvent);    
                };

                return recordsAffected;
            }

            catch (Exception ex)
            {
                //log the event
                
                var updatedFailedEvent = new VoucherUpdateFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now,
                        Message = "Could not perform update on voucher", VoucherCode = code, 
                        FailureReason = ex.Message 
                };

                _logger.LogInformation("Updated a voucher: {@Event}", updatedFailedEvent);    

                
                _logger.LogError(ex, "Could not perform activate or deactivate operation on voucher with {Code}", code);
            }
                return null;
        }

        public async Task<Voucher> UpdateGiftVoucherAmount(string code, long amount)
        {
            try
            {
                var voucher = await GetVoucherByCode(code);
                Gift giftVoucher = await _giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
                
                var previousAmount = giftVoucher.GiftAmount;

                giftVoucher.GiftAmount = amount; // do the update
                await _giftVoucherService.UpdateGiftVoucher(giftVoucher); //persist the change   
    
                //log the event
                var updatedEvent = new VoucherUpdatedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucher.MerchantId,
                        Message = "Update performed on voucher", VoucherCode = voucher.Code, 
                        VoucherType = voucher.VoucherType, PropertyUpdated = new PropertyUpdated() {
                        PropertyName = "GiftAmount",  PreviousValue = previousAmount, NewValue = giftVoucher.GiftAmount}
                    };

                _logger.LogInformation("Updated a voucher: {@UpdateEvent}", updatedEvent);
                return voucher;
            }              
            
            catch (VoucherUpdateException ex)
            {
                var updatedFailedEvent = new VoucherUpdateFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now, VoucherType = VoucherType.GIFT.ToString(),
                        Message = "Update operation failed for voucher", VoucherCode = code, FailureReason = ex.Message,
                        PropertyToUpdate = new PropertyUpdated() {PropertyName = "GiftBalance", NewValue = amount}
                };

                _logger.LogError("Error Updating a gift: {@UpdateFailedEvent}", updatedFailedEvent);

                _logger.LogDebug(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }

        }

        public async Task<long?> UpdateVoucherExpiryDate(string code, DateTime newDate)
        {
            try
            {
                //get the voucher that is to be updated
                var voucher = await GetVoucherByCode(code);
                var oldDate = voucher.ExpiryDate;
                voucher.ExpiryDate = newDate;
                var recordsAffected = await _baseRepository.UpdateVoucherExpiryDateByCodeAsync(voucher);
                
                var updatedEvent = new VoucherUpdatedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucher.MerchantId,
                        Message = "Update performed on voucher", VoucherCode = voucher.Code, 
                        VoucherType = voucher.VoucherType, PropertyUpdated = new PropertyUpdated() {
                            PropertyName = "ExpiryDate", PreviousValue = oldDate, NewValue = voucher.ExpiryDate
                        }
                };

                _logger.LogInformation("Voucher expiry date updated: {@ExpiryUpdateEvent}", updatedEvent);
                return recordsAffected;
            }
            catch (VoucherUpdateException ex)
            {
                var updateFailedEvent = new VoucherUpdateFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now,
                        Message = "Update performed on voucher", VoucherCode = code, 
                        FailureReason = ex.Message, PropertyToUpdate = new PropertyUpdated() {
                            PropertyName = "ExpiryDate", NewValue = newDate
                        }
                };
                _logger.LogError("Failed to update Voucher: {@ExpiryUpdateFailedEvent}", updateFailedEvent);
                _logger.LogDebug(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }
        }

        public Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId)
        {
            try
            {
               return  _giftVoucherService.GetAllGiftVouchers(merchantId);            
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
    }
}
