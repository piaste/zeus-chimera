using Chimera.Authentication.Entities;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios.Extensions;
using ZenProgramming.Chakra.Core.Utilities.Security;

namespace Chimera.Authentication.Mocks.Common
{
    public class SimpleAuthenticationScenario : AuthenticationScenarioBase
    {
        public override void InitializeEntities()
        {
            var user = new User
            {
                UserName = "mauro",
                PasswordHash = ShaProcessor.Sha256Encrypt("P@ssw0rd"),
                FirstName = "Mauro",
                LastName = "Bussini",
                Email = "maurob@icubed.it",
                IsAdministrator = true,
                IsEnabled = true
            };
            this.Push(e => e.Users, user);
        }
    }
}
