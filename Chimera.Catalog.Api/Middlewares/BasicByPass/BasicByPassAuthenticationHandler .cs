using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Chimera.Authentication.Clients;
using Chimera.Authentication.Contracts.Requests;
using Common.Core.Identities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZenProgramming.Chimera.Common.Contracts.DependencyInjectors;

namespace ZenProgramming.Chimera.Catalog.Api.Middlewares.BasicByPass
{
    /// <summary>
    /// Handler for Basi Authentication
    /// </summary>
    public class BasicByPassAuthenticationHandler : AuthenticationHandler<BasicByPassAuthenticationOptions>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="logger">Logger</param>
        /// <param name="encoder">Encoder</param>
        /// <param name="clock">Clock</param>
        public BasicByPassAuthenticationHandler(IOptionsMonitor<BasicByPassAuthenticationOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        /// <summary>
        /// Handle process for current authentication
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Se non ho headers o non ho "Authentication", esco
            if (string.IsNullOrEmpty(Request.Headers?["Authorization"]))
            {
                //Fallisco l'autenticazione
                return AuthenticateResult.Fail("Header 'Authorization' was not provided");
            }

            //Recupero il valore e split
            string authValue = Request.Headers["Authorization"];
            var segments = authValue.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            //Se non ho due elementi, esco
            if (segments.Length != 2)
            {
                //Fallisco l'autenticazione
                return AuthenticateResult.Fail("Header 'Authorization' should contains two items: schema and value");
            }

            //Se il lo schema non è Basic, esco
            if (segments[0] != "Basic" || string.IsNullOrEmpty(segments[1]))
            {
                //Fallisco l'autenticazione
                return AuthenticateResult.Fail($"Provided schema is not '{Scheme.Name}'");
            }

            string credentials;
            try
            {
                //Il valore dell'intestazione va decodificato dalla sua forma Base64
                //Per i dettagli, vedere: http://www.w3.org/Protocols/HTTP/1.0/spec.html#BasicAA
                credentials = Encoding.UTF8.GetString(Convert.FromBase64String(segments[1]));
            }
            catch
            {
                //Probabilmente la stringa base64 non era valida
                credentials = string.Empty;
            }

            //Username e password sono separati dal carattere delimitatore ":"
            //Terminiamo l'esecuzione se non è presente o se è in posizione non valida
            var indexOfSeparator = credentials.IndexOf(":", StringComparison.Ordinal);
            if (indexOfSeparator < 1 || indexOfSeparator > credentials.Length - 2)
            {
                //Fallisco l'autenticazione
                return AuthenticateResult.Fail("Base64 encoded values should be separated by char ':'");
            }

            //Estraiamo finalmente le credenziali
            var username = credentials.Substring(0, indexOfSeparator);
            var password = credentials.Substring(indexOfSeparator + 1);

            //Se username o password sono vuoti, esco
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                //Fallisco l'autenticazione
                return AuthenticateResult.Fail("Username and/or password should not be empty or null");
            }

            //TODO Eseguire risoluzione della dipendenza
            IAuthenticationClient client = NinjectUtils.Resolve<IAuthenticationClient>();

            //Invoke del sevrizio remoto
            var request = new SignInRequest { UserName = username, Password = password };
            var response = await client.SignIn(request);

            //Se non è andato a buon fine (non 200), uscita
            if (!response.Response.IsSuccessStatusCode)
                return AuthenticateResult.Fail("Invalid credentials");

            //Create identity claims using provided data
            ClaimsIdentity claimsIdentity = new ClaimsIdentity("Basic");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, response.Data.UserName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, response.Data.FirstName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, response.Data.LastName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, response.Data.Email));
            claimsIdentity.AddClaim(new Claim("IsAdministrator", response.Data.IsAdministrator.ToString()));

            //OLD VERSION
            //var identity = new GenericIdentity(response.Data.UserName);
            //var principal = new GenericPrincipal(identity, new string[] { });

            //Creo il ticket di autenticazione
            var authTicket = new AuthenticationTicket(
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties(),
                Scheme.Name);

            //Confermo l'autenticazione
            return AuthenticateResult.Success(authTicket);


            ////Istanzio il client per l'autenticazione sul server remoto
            //var client = NinjectUtils.Resolve<IAuthenticationClient>();

            ////Tento di eseguire il sign-in dell'utente
            //var response = await client.SignIn(new SignInRequest
            //{
            //    UserName =  username,
            //    Password  = password
            //});

            ////Se la risposta non è valida, fallisco
            //if (!response.Response.IsSuccessStatusCode && response.Response.StatusCode != HttpStatusCode.Unauthorized)
            //{
            //    //Fallisco
            //    return AuthenticateResult.Fail("Remote authentication server returned invalid response");
            //}

            ////Se è unauthorized, non sono autenticato
            //if (response.Response.StatusCode == HttpStatusCode.Unauthorized || response.Data == null)
            //{
            //    //Fallisco
            //    return AuthenticateResult.Fail("Provided credentials are invalid");
            //}
        }
    }
}