using Chimera.Authentication.Entities;
using ZenProgramming.Chakra.Core.Data.Repositories;

namespace Chimera.Authentication.Data.Repositories
{
    /// <summary>
    /// Interface for repository of User 
    /// </summary>
    public interface IUserRepository: IRepository<User>
    {
        /// <summary>
        /// Get single user using username
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>Returns user or null</returns>
        User GetUserByUserName(string userName);
    }
}
