using Chimera.Authentication.Entities;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;

namespace Chimera.Authentication.Mocks
{
    public interface IAuthenticationScenario: IScenario
    {
        IList<User> Users { get; set; }
    }
}
