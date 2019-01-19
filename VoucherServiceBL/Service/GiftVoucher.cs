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
        
        public Gift CreateGiftVoucher(VoucherRequest giftRequest)
        {
            //create the gift object from the Vouher
            Gift giftVoucher;
            giftVoucher = new Gift() 
                {Code = HashedCode(giftRequest),
                 CreationDate = giftRequest.CreationDate,
                 ExpiryDate = giftRequest.ExpiryDate,
                 VoucherStatus = "Active",
                 VoucherType = giftRequest.VoucherType,
                 Description = giftRequest.Description,
                 GiftAmount = giftRequest.GiftAmount,
                 MerchantId = giftRequest.MerchantId,
                 Metadata = giftRequest.Metadata,
                 GiftBalance = giftRequest.GiftAmount //giftbalance == gift amount at creation
                };

            //persist the object to the db    
            return Repository.CreateGiftVoucher(giftVoucher);
        }

        public Gift GetGiftVoucher(Voucher voucher)
        {
            throw new System.NotImplementedException();
        }

        public Voucher UpdateGiftVoucher(Gift giftVoucher)
        {
            throw new System.NotImplementedException();
        }

        private string HashedCode(VoucherRequest voucherRequest)  //TODO: move to a util class so it can be shared by all services
        {
            string hashedCode; //pattern or length; prefix; suffix
            string characterSet ;
            string code;
            
            if(voucherRequest.CharacterSet.ToLower() == "alphabet")
                characterSet = Constants.ALPHABET_CHARACTERS;
            
            else if(voucherRequest.CharacterSet.ToLower() == "number")
                characterSet = Constants.NUMBER_CHARACTERS;
            else
                characterSet = Constants.ALPHABET_CHARACTERS + Constants.NUMBER_CHARACTERS;
            
            if (!string.IsNullOrEmpty(voucherRequest.CodePattern))
            {
                code = CodeGenerator.GenerateCodeWithPattern(
                    voucherRequest.CodePattern, characterSet, voucherRequest.Separator);
            }

            else //length is specified 
                code = CodeGenerator.GenerateCode(voucherRequest.CodeLength, characterSet);

            if (!string.IsNullOrEmpty(voucherRequest.Prefix))
                code = CodeGenerator.GetCodeWithPrefix(voucherRequest.Prefix, code);

            if (!string.IsNullOrEmpty(voucherRequest.Prefix))
                code = CodeGenerator.GetCodeWithSuffix(code, voucherRequest.Suffix);

            hashedCode = CodeGenerator.Encrypt(code);
            
            return hashedCode;
        }
    }


}