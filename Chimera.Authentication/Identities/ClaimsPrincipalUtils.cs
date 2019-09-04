using Chimera.Authentication.Identities;
using System;
using System.Security.Claims;

namespace Falck.Pulsar.Core.Identities
{
    /// <summary>
    /// Utilities for Claims Principal
    /// </summary>
    public static class ClaimsPrincipalUtils
    {
        /// <summary>
        /// Claim name for authorization header
        /// </summary>
        public const string AuthorizationHeaderClaim = "AuthorizationHeader";

        /// <summary>
        /// Generates claims principal using provided user
        /// </summary>
        /// <param name="authenticationType">Authentication type (ex. "Basic")</param>
        /// <param name="identity">User instance</param>
        /// <param name="authorizationHeader">Authorization header for "bypass"</param>
        /// <returns>Returns claims identity</returns>
        public static ClaimsPrincipal GeneratesClaimsPrincipal(string authenticationType, 
            IGenericIdentity identity, string authorizationHeader = null)
        {
            //Arguments validation
            if (string.IsNullOrEmpty(authenticationType)) throw new ArgumentNullException(nameof(authenticationType));
            if (identity == null) throw new ArgumentNullException(nameof(identity));

            //Create identity claims using provided data
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(authenticationType);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, identity.UserName));

            //With authorization header, create claims
            if (!string.IsNullOrEmpty(authorizationHeader))
                claimsIdentity.AddClaim(new Claim(AuthorizationHeaderClaim, authorizationHeader));

            //Generate and return principal
            return new ClaimsPrincipal(claimsIdentity);
        }        
    }
}