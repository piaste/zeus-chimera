using Chimera.Authentication.Data.Repositories;
using Chimera.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Data.Repositories.Mockups;

namespace Chimera.Authentication.Mocks.Data.Repositories
{
    [Repository]
    public class MockUserRepository : MockupRepositoryBase<User, IAuthenticationScenario>, IUserRepository
    {
        public MockUserRepository(IDataSession dataSession) 
            : base(dataSession, scenario => scenario.Users)
        {
        }

        public User GetUserByUserName(string userName)
        {
            return MockedEntities
                .SingleOrDefault(u => u.UserName.ToLower() == userName.ToLower());
        }
    }
}
