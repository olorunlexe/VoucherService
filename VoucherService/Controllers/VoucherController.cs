using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoucherServiceBL.Domain;

namespace VoucherService.Controllers
{
    [Produces("application/json")]
    [Authorize]
    //[Route("api/[controller]/[action]")]
    [Route("api/v1")]
    [ApiController]
    public class VoucherController : ControllerBase
    {

        [HttpPost]
        public async Task<VoucherRequest> CreateVoucher([FromBody] VoucherRequest voucher)
        {
            return null;
        }

        [HttpPost("{code}")]
        public async Task<VoucherRequest> GetVoucher([FromRoute] string code)
        {
            return null;
        }


        [HttpPost]
        [Route("all")]
        public async Task<VoucherRequest> GetVoucher()
        {
            return null;
        }

        [HttpPut("{code}")]
        public async Task<VoucherRequest> UpdateVoucher([FromRoute] string code, [FromBody] VoucherRequest voucher)
        {
            return null;
        }

        [HttpPatch("{code}")]
        public async Task<VoucherRequest> ChangeVoucherStatus([FromRoute] string code, [FromBody] VoucherRequest voucher)
        {
            return null;
        }


        [HttpDelete("{code}")]
        public async Task<VoucherRequest> DeleteVoucher([FromRoute] string code)
        {
            return null;
        }



    }
}