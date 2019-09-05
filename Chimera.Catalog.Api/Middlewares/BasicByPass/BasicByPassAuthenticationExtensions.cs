using Microsoft.AspNetCore.Authentication;

namespace ZenProgramming.Chimera.Catalog.Api.Middlewares.BasicByPass
{   
    /// <summary>
    /// Extensions for Basic Authentication (with by-pass)
    /// </summary>
    public static class BasicByPassAuthenticationExtensions
    {
        /// <summary>
        /// Add Basic Authentication to the pipeline
        /// </summary>
        /// <param name="builder">Auth builder</param>
        /// <returns>Returns builder with added schema</returns>
        public static AuthenticationBuilder AddBasicByPassAuthentication(this AuthenticationBuilder builder)
        {
            //Aggiungo lo schema di autenticazione con le opzioni
            return builder.AddScheme<BasicByPassAuthenticationOptions, BasicByPassAuthenticationHandler>(
                BasicByPassAuthenticationOptions.Scheme,
                BasicByPassAuthenticationOptions.Scheme, 
                o => { });
        }
    }
}
