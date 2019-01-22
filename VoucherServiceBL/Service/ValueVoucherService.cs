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
    public class ValueVoucherService : IValueVoucherService
    {
        private IValueRepository repository;

        public IValueRepository ValueRepository => this.repository;

        public ValueVoucherService(IValueRepository repository)
        {
            this.repository = repository;
        }

        public Value CreateValueVoucher(VoucherRequest valueRequest)
        {
            //create the gift object from the Vouher
            Value valueVoucher = new Value()
            {
                Code = CodeGenerator.HashedCode(valueRequest),
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
            return ValueRepository.CreateValueVoucher(valueVoucher);
        }

        public List<Value> GetAllValueVouchers(string merchantId)
        {
            return ValueRepository.GetAllValueVouchers(merchantId);
        }

        public Value GetValueVoucher(Voucher voucher)
        {
            return ValueRepository.GetValueVoucher(voucher);
        }

    }
}
