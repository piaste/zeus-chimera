using Chimera.Authentication.Contracts;
using Chimera.Authentication.Contracts.Requests;
using System.Threading.Tasks;
using ZenProgramming.Chakra.Core.Http;

namespace Chimera.Authentication.Clients
{
    public interface IAuthenticationClient
    {
        /// <summary>
        /// Executes sign-in on remote platform
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Returns response</returns>
        Task<HttpResponseMessage<UserContract>> SignIn(SignInRequest request);
    }
}
