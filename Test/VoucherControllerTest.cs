using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoucherService.Controllers;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Service;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task GetVoucher_WhenCalledWithCorrectCode_ShouldReturnCorrectVoucher()
        {

            // Arrange
            var testVoucher = GetVoucher();
            var mockService = new Mock<IVoucherService>();
            mockService.Setup(service => service.GetVoucherByCode("25142dw")).Returns(testVoucher);
            var controller = new VoucherController(mockService.Object);

            // Act
            var result = await controller.GetVoucher("25142dw");
 
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testVoucher, result);

        }

        [Test]
        public async Task GetVoucher_WhenCalledWithWrongCode_ShouldNotReturnNull()
        {
            //Arrange
            var testVoucher = GetVoucherByCode("sgwt3683ydg");
            var mockService = new Mock<IVoucherService>();
            mockService.Setup(service => service.GetVoucherByCode("sgwt3683ydg")).Returns(testVoucher);
            var controller = new VoucherController(mockService.Object);

            //Act
            var result = await controller.GetVoucher("sgwt3683ydg");

            //Assert
            Assert.IsNull(result);
            Assert.AreEqual(testVoucher, result);
        }

        [Test]
        public async Task GetAllVoucher_WhenCalledWithCorrectMerchantId_ShouldReturnAListOfAllVouchersAttachedToTheMerchant()
        {

            // Arrange
            var testVouchers = GetVouchersByMerchantId("2222");
            var mockService = new Mock<IVoucherService>();
            mockService.Setup(service => service.GetAllVouchers("2222")).Returns(testVouchers);
            var controller = new VoucherController(mockService.Object);

            // Act
            var result = await controller.GetAllVouchers("2222");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testVouchers, result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllVoucher_WhenCalledWithWrongMerchantId_ShouldReturnAnyVoucher()
        {

            // Arrange
            var testVouchers = GetVouchersByMerchantId("6234");
            var mockService = new Mock<IVoucherService>();
            mockService.Setup(service => service.GetAllVouchers("6234")).Returns(testVouchers);
            var controller = new VoucherController(mockService.Object);

            // Act
            var result = await controller.GetAllVouchers("6234");

            // Assert
            Assert.AreEqual(testVouchers, result);
            Assert.AreEqual(0, result.Count());
        }



        //[Test]
        //public async Task DeleteVoucher_ShouldChangeVoucherStatusToDeleted()
        //{
        //    // Arrange
        //    var testVoucher = GetVoucher();
        //    var mockService = new Mock<IVoucherService>();
        //    mockService.Setup(service => service.DeleteVoucher("25142dw"));
        //    var controller = new VoucherController(mockService.Object);

        //    // Act
        //    var result = controller.DeleteVoucher("25142dw");

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Deleted", result.VoucherStatus);

        //}
        private IEnumerable<Value> GetVouchers()
        {
            var TestValueVouchers = new List<Value>();
            TestValueVouchers.Add(new Value { Code = "253hjD142dw", Description = null, CreationDate = new DateTime(2019, 09, 15), ExpiryDate = new DateTime(2019, 12, 25), MerchantId = "2222", ValueAmount = 2000, VoucherStatus = "Active", VoucherType = "Value", Metadata = null });
            TestValueVouchers.Add(new Value { Code = "zrd352j", Description = null, CreationDate = new DateTime(2019, 01, 20), ExpiryDate = new DateTime(2019, 01, 29), MerchantId = "2222", ValueAmount = 1000, VoucherStatus = "Active", VoucherType = "Value", Metadata = null });
            TestValueVouchers.Add(new Value { Code = "36632fjms", Description = null, CreationDate = new DateTime(2019, 10, 28), ExpiryDate = new DateTime(2019, 10, 30), MerchantId = "2223", ValueAmount = 500, VoucherStatus = "Active", VoucherType = "Value", Metadata = null });
            TestValueVouchers.Add(new Value { Code = "ytw5353ss", Description = null, CreationDate = new DateTime(2019, 12, 12), ExpiryDate = new DateTime(2019, 12, 14), MerchantId = "2224", ValueAmount = 1500, VoucherStatus = "Active", VoucherType = "Value", Metadata = null });
            return TestValueVouchers;
        }

        private Voucher GetVoucher()
        {
            var voucher = new Voucher { Id = 0, Code = "25142dw", Description = null, CreationDate = new DateTime(2019, 09, 15), ExpiryDate = new DateTime(2019, 12, 25), MerchantId = "2222", VoucherStatus = "Active", VoucherType = "Value", Metadata = null };
            return voucher;
        }

        private Voucher GetVoucherByCode(string code)
        {
            var vouchers = GetVouchers();
            var voucher = from v in vouchers where v.Code == code select v;
            return voucher.FirstOrDefault<Value>();
        }

        private IEnumerable<Value> GetVouchersByMerchantId(String merchantId)
        {
            var values = GetVouchers();
            var value = from v in values where v.MerchantId == merchantId select v;
            return value.AsEnumerable<Value>();
        }
        
    }
}