using System.Net;
using System.Threading.Tasks;
using Chimera.Authentication.Contracts;
using Chimera.Authentication.Contracts.Requests;
using ZenProgramming.Chakra.Core.Http;

namespace Chimera.Authentication.Clients.Mocks
{
    public class MockAuthenticationClient : IAuthenticationClient
    {
        public MockAuthenticationClient()
        {
        }

        public Task<HttpResponseMessage<UserContract>> SignIn(SignInRequest request)
        {
            var response = new HttpResponseMessage<UserContract>(
                new System.Net.Http.HttpResponseMessage(HttpStatusCode.OK),
                new UserContract
                {
                    UserName = request.UserName, 
                    LastName = "FakeLastName", 
                    FirstName = "FakeFirstName", 
                    Email = "fake@mock.it", 
                    IsAdministrator = false
                });
            return Task.FromResult(response);
        }
    }
}
