using System;
using System.Collections.Generic;
using System.Text;
using Chimera.Authentication.Data.Repositories;
using Chimera.Authentication.Entities;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Utilities.Security;

namespace Chimera.Authentication.ServiceLayers
{
    public class AuthenticationServiceLayer
    {
        private readonly IUserRepository _UserRepository;

        public AuthenticationServiceLayer(IDataSession session)
        {
            //Risoluzione delle dipendenze
            _UserRepository = session.ResolveRepository<IUserRepository>();
        }

        public User SignIn(string userName, string password)
        {
            //Recuperare utente con username indicata da database
            User user = GetUserByUserName(userName);

            //Se non ho utente, esco
            if (user == null)
                return null;

            //Se è disabilitato, esco
            if (!user.IsEnabled)
                return null;

            //Enconding della password passata
            var encondedPassword = ShaProcessor.Sha256Encrypt(password);

            //Se le password non sono corrette, null
            //altrimenti emetto l'utente
            return encondedPassword == user.PasswordHash 
                ? user 
                : null;
        }

        /// <summary>
        /// Get single user using provided username
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>Returns user or null</returns>
        private User GetUserByUserName(string userName)
        {
            //Utilizzo direttamente il repo
            return _UserRepository.GetUserByUserName(userName);
        }
    }
}
