using Common.Contracts.Identities;
using System;
using System.Security.Claims;

namespace Common.Core.Identities
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
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, identity.FirstName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, identity.LastName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, identity.Email));
            claimsIdentity.AddClaim(new Claim("IsAdministrator", identity.IsAdministrator.ToString()));

            //With authorization header, create claims
            if (!string.IsNullOrEmpty(authorizationHeader))
                claimsIdentity.AddClaim(new Claim(AuthorizationHeaderClaim, authorizationHeader));

            //Generate and return principal
            return new ClaimsPrincipal(claimsIdentity);
        }        
    }
}