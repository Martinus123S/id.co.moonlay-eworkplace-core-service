using EWorkplaceCoreService.Lib;
using EWorkplaceCoreService.Lib.Helpers.IdentityService;
using EWorkplaceCoreService.Lib.Models;
using EWorkplaceCoreService.Lib.Services.Divisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EWorkplaceCoreService.Test.Services
{
    public class DivisionServiceTest
    {
        private const string ENTITY = "JournalTransaction";
        //private PurchasingDocumentAcceptanceDataUtil pdaDataUtil;
        //private readonly IIdentityService identityService;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private CoreDbContext GetDbContext(string testName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return new CoreDbContext(optionsBuilder.Options);
        }

        private Mock<IServiceProvider> GetServiceProviderMock()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { TimezoneOffset = 1, Token = "token", Username = "username" });
            return serviceProviderMock;
        }

        private Division GetNewModel()
        {
            return new Division()
            {
                Code = "DivisionCode",
                Name = "DivisionName"
            };
        }

        [Fact]
        public async Task Should_Success_Create_Valid_Model()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();

            var service = new DivisionService(dbContext, serviceProviderMock.Object);
            var model = GetNewModel();

            var result = await service.Create(model);

            Assert.NotEqual(result, 0);
        }

        [Fact]
        public async Task Should_Success_Delete_Valid_Id()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();

            var service = new DivisionService(dbContext, serviceProviderMock.Object);
            var model = GetNewModel();

            await service.Create(model);

            var result = await service.Delete(model.Id);

            Assert.NotEqual(result, 0);
        }

        [Fact]
        public async Task Should_Error_Delete_InValid_Id()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();

            var service = new DivisionService(dbContext, serviceProviderMock.Object);
            var model = GetNewModel();

            await service.Create(model);

            await Assert.ThrowsAsync<Exception>(() => service.Delete(model.Id + 1));
        }
    }
}
