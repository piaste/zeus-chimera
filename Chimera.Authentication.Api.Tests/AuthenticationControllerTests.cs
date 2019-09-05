using Chimera.Authentication.Api.Controllers;
using Chimera.Authentication.Contracts;
using Chimera.Authentication.Contracts.Requests;
using Chimera.Authentication.Mocks.Common;
using Common.Api.Tests;
using Common.Core.Identities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace Chimera.Authentication.Api.Tests
{
    public class AuthenticationControllerTests: ApiControllerTestsBase<AuthenticationController, SimpleAuthenticationScenario>
    {
        protected override IGenericIdentity GetIdentity()
        {
            return Scenario.Users.First();
        }

        [Fact]
        public void ShouldSignInBeOkWithValidCredentials()
        {
            //1) Setup

            //2) Execution
            var request = new SignInRequest { UserName = "mauro", Password = "P@ssw0rd" };
            IActionResult response = Controller.SignIn(request);
            var parsed = ParseExpectedOk<UserContract>(response);

            //3) Assert
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.Response);
            Assert.Equal("Mauro", parsed.Data.FirstName);
            Assert.Equal("Bussini", parsed.Data.LastName);
            
            //4) Cleanup
            // In questo caso non esiste pulizia
        }

        [Fact]
        public void ShouldSignInBeBadRequestWithInvalidUserName()
        {
            //1) Setup

            //2) Execution
            var request = new SignInRequest { UserName = "4567890545", Password = "P@ssw0rd" };
            IActionResult response = Controller.SignIn(request);
            var parsed = ParseExpectedUnauthorized(response);

            //3) Assert
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.Response);
        }

        
    }
}
