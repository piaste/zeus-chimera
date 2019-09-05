using Microsoft.AspNetCore.Authentication;

namespace ZenProgramming.Chimera.Catalog.Api.Middlewares.BasicByPass
{
    /// <summary>
    /// Options for Basic Authentication
    /// </summary>
    public class BasicByPassAuthenticationOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Schema name
        /// </summary>
        public static string Scheme = "Basic";
    }
}
