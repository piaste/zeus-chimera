namespace Common.Contracts.Identities
{
    public interface IGenericIdentity
    {
        string UserName { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }

        bool IsAdministrator { get; set; }
    }
}
