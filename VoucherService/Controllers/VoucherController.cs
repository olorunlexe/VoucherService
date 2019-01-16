using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VoucherService.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]/[action]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {

            [HttpGet]
            public async Task<ActionResult> CreateVoucher(string code)
            {
                return await Task.Run(() => new JsonResult("Welcome to CreateVoucher Endpoint"));
            }

            [HttpDelete("{code}")]
            public async Task<ActionResult> DeleteVoucher(string code)
            {
                return new JsonResult(Response);
            }

            [HttpGet]
            public async Task<ActionResult> GetVoucherList()
            {
                return new JsonResult(Response);
            }

            [HttpPost("{code}")]
            public async Task<ActionResult> EnableVoucher(string code)
            {
                return new JsonResult(Response);
            }

            [HttpPost("{code}")]
            public async Task<ActionResult> DisableVoucher(string code)
            {
                return new JsonResult(Response);
            }

            [HttpPost("{code}")]
            public async Task<ActionResult> AddGiftVoucherBalance(string code)
            {
                return new JsonResult(Response);
            }

            [HttpPost]
            public async Task<ActionResult> ImportVouchers()
            {
                return new JsonResult(Response);
            }

            [HttpPost]
            public async Task<ActionResult> ImportVouchersByCSV()
            {
                return new JsonResult(Response);
            }

        }
}