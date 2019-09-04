using System;
using System.Collections.Generic;
using System.Text;

namespace Chimera.Authentication.Identities
{
    public interface IGenericIdentity
    {
        string UserName { get; set; }
    }
}
