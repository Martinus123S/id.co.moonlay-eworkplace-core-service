using Com.Moonlay.NetCore.Lib.Service;
using EWorkplaceCoreService.Lib.Helpers.IdentityService;
using EWorkplaceCoreService.Lib.Helpers.ValidateService;
using EWorkplaceCoreService.Lib.Models;
using EWorkplaceCoreService.Lib.Services.Divisions;
using EWorkplaceCoreService.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EWorkplaceCoreService.Test.Controllers
{
    public class DivisionControllerTest
    {
        private Mock<IServiceProvider> GetServiceProviderMock(IDivisionService divisionService, IValidateService validateService)
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IDivisionService)))
                .Returns(divisionService);

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService)))
                .Returns(validateService);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { TimezoneOffset = 1, Token = "token", Username = "username" });


            return serviceProviderMock;
        }

        private DivisionController GetController(IServiceProvider serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            var controller = (DivisionController)Activator.CreateInstance(typeof(DivisionController), serviceProvider);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");

            return controller;
        }

        private int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        private ServiceValidationExeption GetServiceValidationException()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(new Division(), serviceProvider.Object, null);
            return new ServiceValidationExeption(validationContext, validationResults);
        }


        [Fact]
        public async Task Should_Success_Post_Data()
        {
            var divisionServiceMock = new Mock<IDivisionService>();
            divisionServiceMock.Setup(divisionService => divisionService.Create(It.IsAny<Division>()))
                .ReturnsAsync(1);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<Division>()))
                .Verifiable();

            var serviceProviderMock = GetServiceProviderMock(divisionServiceMock.Object, validateServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);
            var response = await controller.Post(new Division() { Code = "1", Name = "DivisionName" });

            Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Success_Post_Invalid_Data()
        {
            var divisionServiceMock = new Mock<IDivisionService>();
            divisionServiceMock.Setup(divisionService => divisionService.Create(It.IsAny<Division>()))
                .ReturnsAsync(1);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock.Setup(validateService => validateService.Validate(It.IsAny<Division>()))
                .Throws(GetServiceValidationException());

            var serviceProviderMock = GetServiceProviderMock(divisionServiceMock.Object, validateServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);
            var response = await controller.Post(new Division());

            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }
    }
}
