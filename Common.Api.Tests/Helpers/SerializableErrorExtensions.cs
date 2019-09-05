using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Tests.Helpers
{
    /// <summary>
    /// Extensions for serializable error
    /// </summary>
    public static class SerializableErrorExtensions
    {
        /// <summary>
        /// Checks if message with provided text exists on SerializableError
        /// </summary>
        /// <param name="instance">Instance</param>
        /// <param name="message">Message</param>
        /// <returns>Returns true or false</returns>
        public static bool HasMessage(this SerializableError instance, string message)
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

            //Recupero i messaggi
            var messages = GetMessages(instance);

            //Verifico se il messaggio esiste
            return messages.Any(m => m == message);
        }

        /// <summary>
        /// Fetch list of messages contained on SerializableError
        /// </summary>
        /// <param name="instance">Instance</param>
        /// <returns>Returns list of messages</returns>
        public static IList<string> GetMessages(this SerializableError instance)
        {
            //Validazione argomenti
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //Lista dei messaggi in uscita
            var outList = new List<string>();

            //Iterazione dei valori
            foreach (var currentElement in instance)
            {
                //Il contenuto del KeyValuePair è un oggetto
                //quindi è necessario fare la trasformazione in 
                //string[]; se il parse non è possibile, salto
                if (!(currentElement.Value is string[] parsed))
                    continue;

                //Iterazione sulle stringhe
                foreach (var currentMessage in parsed)
                {
                    //Aggiunta dei messaggi
                    outList.Add(currentMessage);
                }
            }

            //Emissione
            return outList;
        }
    }
}
