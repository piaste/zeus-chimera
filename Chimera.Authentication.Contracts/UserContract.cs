using Common.Contracts.Identities;

namespace Chimera.Authentication.Contracts
{
    public class UserContract: IGenericIdentity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public bool IsAdministrator { get; set; }

        public string Email { get; set; }
    }
}
