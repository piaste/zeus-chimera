using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chimera.Authentication.Api.Models
{
    public class UserContract
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public bool IsAdmnistrator { get; set; }

        public string Email { get; set; }
    }
}
