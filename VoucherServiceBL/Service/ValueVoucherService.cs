using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

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

        public Task<int> CreateValueVoucher(VoucherRequest valueRequest)
        {
            // var numOfVouchersCreated = 0;

            var vouchersList = new List<Value>(valueRequest.NumbersOfVoucherToCreate);

            //create the gift object from the Vouher
            foreach (var num in Enumerable.Range(1, valueRequest.NumbersOfVoucherToCreate))

            {
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
                vouchersList.Add(valueVoucher);
            }

                //persist the object to the db    
                return ValueRepository.CreateValueVoucher(vouchersList);
        }

        public Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId)
        {
            return ValueRepository.GetAllValueVouchers(merchantId);
        }

        public Task<Value> GetValueVoucher(Voucher voucher)
        {
            return ValueRepository.GetValueVoucher(voucher);
        }

    }
}
