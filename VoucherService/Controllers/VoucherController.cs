using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;
using VoucherServiceBL.Service;


namespace VoucherService.Controllers
{
    [Produces("application/json")]
    //[Authorize]
    //[Route("api/[controller]/[action]")]
    [Route("api/v1")]
    [ApiController]
    public class VoucherController : ControllerBase
    {

        // private IGiftVoucher giftVoucher; //TODO: remove
        // private IDiscountVoucher discountVoucher; //TODO: remove
        private IVoucherService baseVoucherService;
        public VoucherController(
                IVoucherService baseService)
        {
            // this.giftVoucher = giftService;
            // this.discountVoucher = discountService;
            this.baseVoucherService = baseService;
        }

        /// <summary>
        /// Create voucher(s) passing in the wanted properties
        /// </summary>
        /// <param name="voucherReq">the object containing the wanted properties of the voucher(s)
        /// to create
        /// </param>
        /// <returns>the created voucher</returns>

        [HttpPost]
        public async Task<Voucher> CreateVoucher([FromBody] VoucherRequest voucherReq)
        {
            return baseVoucherService.CreateVoucher(voucherReq);
        }

        /// <summary>
        /// Retrieve a single voucher 
        /// </summary>
        /// <param name="code">code of the voucher to retrieve</param>
        /// <returns>voucher with a matching code</returns>
        [HttpGet("{code}")]
        public async Task<Voucher> GetVoucher([FromRoute] string code)
        {
            return baseVoucherService.GetVoucherByCode(code);
        }

        /// <summary>
        /// Retrieve all the vouchers created the by a merchant
        /// </summary>
        /// <param name="merchantId">the id of the merchant whose vouchers are to be retrieved</param>
        /// <returns></returns>

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<Voucher>> GetAllVouchers([FromQuery] string merchantId)
        {
            return baseVoucherService.GetAllVouchers(merchantId);
        }

        /// <summary>
        /// update the properties of a voucher
        /// </summary>
        /// <param name="code">code of the voucher to update</param>
        /// <param name="voucherUpdateReq">the object carrying the update</param>
        /// <returns></returns>

        [HttpPut]
        public async Task<Voucher> UpdateVoucher([FromBody] VoucherUpdateReq voucher)
        {
            return baseVoucherService.UpdateVoucher(voucher);
        }


        /// <summary>
        /// Call this end point to disable a voucher
        /// </summary>
        /// <param name="code">code of the voucher to ou </param>
        /// <param name="voucherUpdateReq">the object carrying the update</param>
        /// <returns></returns>
        [HttpPatch("{code}")]
        public async Task EnableOrDisableVoucher([FromRoute] string code, [FromBody] VoucherUpdateReq voucherUpdateReq)
        {
                baseVoucherService.UpdateVoucher(voucherUpdateReq);
        }

        /// <summary>
        /// Delete a voucher created by a merchant
        /// </summary>
        /// <param name="code">code of the voucher to delete</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        public async Task DeleteVoucher([FromRoute] string code)
        {
            baseVoucherService.DeleteVoucher(code);
        }

    }
}