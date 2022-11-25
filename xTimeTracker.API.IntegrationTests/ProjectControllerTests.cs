using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using xTimeTracker.Core.Repositories;
using xTimeTracker.DataAccess.MSSQL.Repositories;

namespace xTimeTracker.API.IntegrationTests
{
    public class ProjectControllerTests
    {
        [Fact]
        public async Task Get_SendRequest_ShouldReturnOkStatus()
        {
            //Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();
            //Act
            var respone = await client.GetAsync("api/project");
            respone.EnsureSuccessStatusCode();
            //Assert
            Assert.Equal(HttpStatusCode.OK, respone.StatusCode);
        }
    }
}