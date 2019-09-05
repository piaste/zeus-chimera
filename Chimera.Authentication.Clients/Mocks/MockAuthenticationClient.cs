using System.Collections.Generic;
using System.Linq;
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
            Users = new List<UserContract>()
            {
                new UserContract
                {
                    UserName = "mario.rossi",
                    FirstName = "Mario",
                    LastName = "Rossi",
                    Email = "mario@rossi.it",
                    IsAdministrator = true
                }
            };
        }

        public IList<UserContract> Users { get; set; }

        public Task<HttpResponseMessage<UserContract>> SignIn(SignInRequest request)
        {
            //Ricerca dell'utente per username
            var user = Users.SingleOrDefault(e => e.UserName.ToLower() == request.UserName.ToLower());

            //Se non è stato trovato, 401
            if (user == null)
                return Task.FromResult(HttpResponseMessage<UserContract>.Unauthorized());

            //Altrimenti compongo un 200
            var response = new HttpResponseMessage<UserContract>(
                new System.Net.Http.HttpResponseMessage(HttpStatusCode.OK),
                user);
            return Task.FromResult(response);
        }
    }
}
