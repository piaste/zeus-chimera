using System.Net.Http;
using System.Threading.Tasks;
using Chimera.Authentication.Contracts;
using Chimera.Authentication.Contracts.Requests;
using ZenProgramming.Chakra.Core.Http;
using ZenProgramming.Chimera.Common.Client.Http;

namespace Chimera.Authentication.Clients.Http
{
    public class HttpAuthenticationClient : HttpClientBase, IAuthenticationClient
    {
        public static string BaseUrl = "";

        public HttpAuthenticationClient() : base(BaseUrl) { }

        public Task<HttpResponseMessage<UserContract>> SignIn(SignInRequest request)
        {
            return Invoke<SignInRequest, UserContract>("api/Authentication/SignIn",
                HttpMethod.Post, request, null);
        }
    }
}
