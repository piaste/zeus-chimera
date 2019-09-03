using Chimera.Authentication.Api.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chimera.Authentication.Api.Tests
{
    public abstract class ApiControllerTestsBase
    {
        /// <summary>
        /// Parses provided response as "Ok - 200" and returns contained data
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <param name="response">Response to parse</param>
        /// <returns>Returns parsed response</returns>
        protected ActionResultStructure<OkObjectResult, TData> ParseExpectedOk<TData>(IActionResult response)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Mi attendo un 200
            if (!(response is OkObjectResult castedResponse))
                throw new InvalidProgramException($"Response should be of type {typeof(OkObjectResult).Name}");

            //Attendo che il risultato del tipo atteso
            if (!(castedResponse.Value is TData castedResult))
                throw new InvalidProgramException($"Response data should be of type {typeof(TData).FullName}");

            //Se i cast sono avvenuti con successo, ritorno la struttura
            return new ActionResultStructure<OkObjectResult, TData>
            {
                Response = castedResponse,
                Data = castedResult
            };
        }
    }
}
