using System;
using System.Collections.Generic;
using System.Text;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.ValueVoucher;
using VoucherServiceBL.ValueVoucher.Repository;

namespace VoucherServiceBL.Service
{
    class ValueVoucher : IValueVoucher
    {
        public IValueRepository Repository;

        public ValueVoucher(IValueRepository repository)
        {
            this.Repository = repository;
        }

        public Value CreateValueVoucher(VoucherRequest valueRequest)
        {
            //create the gift object from the Vouher
            Value giftVoucher;
            giftVoucher = new Value()
            {
                Code = HashedCode(valueRequest),
                CreationDate = valueRequest.CreationDate,
                ExpiryDate = valueRequest.ExpiryDate,
                VoucherStatus = "Active",
                VoucherType = valueRequest.VoucherType,
                Description = valueRequest.Description,
                ValueAmount = valueRequest.ValueAmount,
                MerchantId = valueRequest.MerchantId,
                Metadata = valueRequest.Metadata,
            };

            //persist the object to the db    
            return Repository.CreateValueVoucher(giftVoucher);
        }

        public List<Value> GetAllValueVouchers()
        {
            throw new NotImplementedException();
        }
        
    }
}
