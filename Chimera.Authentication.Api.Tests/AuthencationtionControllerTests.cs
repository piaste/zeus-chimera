using Chimera.Authentication.Api.Controllers;
using Chimera.Authentication.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace Chimera.Authentication.Api.Tests
{
    public class AuthencationtionControllerTests: ApiControllerTestsBase
    {
        [Fact]
        public void ShouldSignInBeOkWithValidCredentials()
        {
            //1) Setup
            var controller = new AuthenticationController();

            //2) Execution
            IActionResult response = controller.SignIn("mauro", "P@ssw0rd");
            var parsed = ParseExpectedOk<UserContract>(response);

            //3) Assert
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.Response);
            Assert.Equal("Mauro", parsed.Data.FirstName);
            Assert.Equal("Bussini", parsed.Data.LastName);


            //4) Cleanup

        }
    }
}
