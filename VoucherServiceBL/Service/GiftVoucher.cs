using System.Collections.Generic;
using VoucherService.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public class GiftVoucher : IGiftVoucher
    {
        public IGiftRepository Repository ;

        public GiftVoucher(IGiftRepository repository)
        {
            this.Repository = repository;
        }
        
        public Gift CreateGiftVoucher(VoucherRequest gift)
        {
            
            Repository.CreateGiftVoucher();
        }

        public IEnumerable<Gift> GetAllGiftVouchers()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateGiftVoucherAmount()
        {
            throw new System.NotImplementedException();
        }
    }

}