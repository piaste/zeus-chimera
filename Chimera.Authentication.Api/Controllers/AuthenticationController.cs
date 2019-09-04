using Chimera.Authentication.Api.Models;
using Chimera.Authentication.Api.Models.Requests;
using Chimera.Authentication.Entities;
using Chimera.Authentication.ServiceLayers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenProgramming.Chakra.Core.Data;

namespace Chimera.Authentication.Api.Controllers
{
    [Route("api/Authentication")]
    public class AuthenticationController: Controller
    {
        private readonly AuthenticationServiceLayer _Layer;

        public AuthenticationController()
        {
            //Inizializzazione della data session
            var session = SessionFactory.OpenSession();
            _Layer = new AuthenticationServiceLayer(session);
        }

        /// <summary>
        /// Executes sign-in on platform
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Returns response with user data<returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn([FromBody]SignInRequest request)
        {
            //Esecuzione della login con username e password
            User user = _Layer.SignIn(request.UserName, request.Password);

            //Se non ho un utente, esco con 401
            if (user == null)
                return Unauthorized();

            //Se ho un utente, converto in contratto
            UserContract contract = new UserContract
            {
                FirstName = user.FirstName, 
                LastName = user.LastName, 
                UserName = user.UserName, 
                IsAdmnistrator = user.IsAdministrator, 
                Email = user.Email              
            };

            //Erogazione response con 200
            return Ok(contract);
        }
    }
}
