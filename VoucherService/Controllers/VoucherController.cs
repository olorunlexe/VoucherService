using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Service;


namespace VoucherService.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/v1")]
    [ApiController]
    public class VoucherController : ControllerBase
    {


        private IVoucherService baseVoucherService;

        private IValueVoucherService valueVoucherService;
        public VoucherController(
                IVoucherService baseService)
        {
            // this.giftVoucher = giftService;
            // this.discountVoucher = discountService;
            this.baseVoucherService = baseService;
        }

        [HttpPost]
        public async Task<Voucher> CreateVoucher([FromBody] VoucherRequest voucherReq)
        {
            return baseVoucherService.CreateVoucher(voucherReq);
        }

        [HttpGet("{code}")]
        public async Task<Voucher> GetVoucher([FromRoute] string code)
        {
            return baseVoucherService.GetVoucherByCode(code);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<Voucher>> GetAllVouchers([FromQuery] string merchantId)
        {
            return baseVoucherService.GetAllVouchers(merchantId);
        }

        [HttpGet("discount/{code}")]
        public async Task<Discount> GetDiscountVoucher([FromRoute] string code)
        {
            return baseVoucherService.GetDiscountVoucher(code);
        }

        [HttpGet]
        [Route("discount/all")]
        public async Task<IEnumerable<Discount>> GetAllDiscountVouchers([FromQuery] string merchantId)
        {
            return baseVoucherService.GetAllDiscountVouchers(merchantId);
        }

        [HttpGet("gift/{code}")]
        public async Task<Gift> GetGiftVoucher([FromRoute] string code)
        {
            return baseVoucherService.GetGiftVoucher(code);
        }


        [HttpGet]
        [Route("gift/all")]
        public async Task<IEnumerable<Gift>> GetAllGiftVouchers([FromQuery] string merchantId)
        {
            return baseVoucherService.GetAllGiftVouchers(merchantId);
        }


        [HttpGet("value/{code}")]
        public async Task<Value> GetValueVoucher([FromRoute] string code)
        {
            return baseVoucherService.GetValueVoucher(code);
        }


        [HttpGet]
        [Route("value/all")]
        public async Task<IEnumerable<Value>> GetAllValueVouchers([FromQuery] string merchantId)
        {
            return baseVoucherService.GetAllValueVouchers(merchantId);
        }

        [HttpPatch("update/{code}")]
        public async Task UpdateVoucherStatus([FromRoute] string code)
        {
                baseVoucherService.ActivateOrDeactivateVoucher(code);
        }

        [HttpPatch("expiry/{code}")]
        public async Task UpdateVoucherExpiryDate([FromRoute] string code, [FromQuery] DateTime newDate)
        {
            baseVoucherService.UpdateVoucherExpiryDate(code,newDate);
        }


        [HttpPatch("amount/{code}")]
        public async Task UpdateGiftVoucherAmount([FromRoute] string code, [FromQuery] long amount)
        {
            baseVoucherService.UpdateGiftVoucherAmount(code,amount);
        }

        [HttpDelete("{code}")]
        public async Task DeleteVoucher([FromRoute] string code)
        {
            baseVoucherService.DeleteVoucher(code);
        }

    }
}