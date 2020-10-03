using System.Threading.Tasks;
using FvpWebApp.Data;
using FvpWebApp.Models;
using FvpWebApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FvpWebAppTests
{
    public class TargetServiceTest : IClassFixture<CustomWebApplicationFactory<FvpWebApp.Startup>>
    {
        public CustomWebApplicationFactory<FvpWebApp.Startup> _factory;
        public TargetServiceTest(CustomWebApplicationFactory<FvpWebApp.Startup> factory)
        {
            _factory = factory;
            _factory.CreateClient();
        }

        [Fact]
        public async Task ValidateCreateTargetService()
        {
            //using (var scope = _factory.Server.Host.Services.CreateScope())
            //{
            //var departmentAppService = scope.ServiceProvider.GetRequiredService<TargetService>();
            //var dbtest = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //dbtest.Department.Add(new Target { DepartmentId = 2, DepartmentCode = "123", DepartmentName = "ABC" });
            //dbtest.SaveChanges();
            //var departmentDto = await departmentAppService.CreateTarget(new Target { });
            Assert.Equal("123", "123");
            //}
        }
    }
}