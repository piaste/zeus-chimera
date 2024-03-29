﻿using Chimera.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chimera.Authentication.Mocks.Common
{
    public abstract class AuthenticationScenarioBase : IAuthenticationScenario
    {
        public IList<User> Users { get; set; }

        public AuthenticationScenarioBase()
        {
            //Inizializzazione
            Users = new List<User>();
        }

        public abstract void InitializeEntities();

        public void InitializeAssets()
        {            
        }

        
    }
}
