using Common.Api.Tests.Helpers;
using Common.Core.Identities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Mockups;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;

namespace Common.Api.Tests
{
    /// <summary>
    /// Base asbtract class for test API controller
    /// </summary>
    /// <typeparam name="TApiController">Type of API controller</typeparam>
    /// <typeparam name="TScenario">Type of scenario to use</typeparam>
    public abstract class ApiControllerTestsBase<TApiController, TScenario> : IDisposable
        where TApiController : Controller, new()
        where TScenario : class, IScenario, new()
    {
        #region Private fields
        private bool _IsDisposed;
        #endregion

        #region Protected properties
        /// <summary>
        /// Scenario used on tests
        /// </summary>
        protected TScenario Scenario { get; set; }

        /// <summary>
        /// Controller instance
        /// </summary>
        protected TApiController Controller { get; set; }

        /// <summary>
        /// Used identity user
        /// </summary>
        protected IGenericIdentity CurrentIdentityUser { get; set; }

        /// <summary>
        /// Random seed
        /// </summary>
        protected Random RandomSeed { get; private set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiControllerTestsBase()
        {
            //Initialize test class
            ExecuteInitialization();
        }

        /// <summary>
        /// Overridable extra initialization
        /// </summary>
        protected virtual void OnInitialize()
        {
            //Default implementation is blank
        }

        /// <summary>
        /// Get identity that will be used for ASP.NET Identity
        /// </summary>
        /// <returns>Returns user instance</returns>
        protected abstract IGenericIdentity GetIdentity();

        /// <summary>
        /// Execute initialization
        /// </summary>
        private void ExecuteInitialization()
        {
            //Do extra initialize
            OnInitialize();

            //Inizializzo il randomizzatore
            RandomSeed = new Random();

            //Inizializzazione dello scenario (usando il tipo)
            TScenario scenario = new TScenario();

            //Imposto lo scenario base per i test
            Scenario = scenario;

            //Inizializzazione dello scenario
            ScenarioFactory.Initialize(Scenario);

            //Registrazione della sessione di default
            SessionFactory.RegisterDefaultDataSession<MockupDataSession>();

            //Creazione del controller dichiarato
            Controller = new TApiController();

            //Recupero l'utente da usare nel testa
            var defaultUserIdentity = GetIdentity();
            if (defaultUserIdentity == null)
                throw new InvalidProgramException("User for identity is invalid");

            //Inizializzazione del controller context e impostazione dell'identity
            UpdateIdentity(defaultUserIdentity);
        }

        /// <summary>
        /// Updates identity set on controller context
        /// </summary>
        /// <param name="user">User instance</param>
        protected void UpdateIdentity(IGenericIdentity user)
        {
            //Validazione argomenti
            if (user == null) throw new ArgumentNullException(nameof(user));

            //impostazione local dell'identità
            CurrentIdentityUser = user;

            //Generazione del principal
            var identity = ClaimsPrincipalUtils.GeneratesClaimsPrincipal("Mock", CurrentIdentityUser);

            Controller.ControllerContext = new ControllerContext
            {
                //HTTP context default
                HttpContext = new DefaultHttpContext
                {
                    //Imposto l'identity generata
                    User = identity
                }
            };
        }

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

        /// <summary>
        /// Parses provided response as "NotFound - 404"
        /// </summary>
        /// <param name="response">Response to parse</param>
        /// <returns>Returns parsed response</returns>
        protected ActionResultStructure<NotFoundResult, object> ParseExpectedNotFound(IActionResult response)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Mi attendo un 404
            if (!(response is NotFoundResult castedResponse))
                throw new InvalidProgramException($"Response should be of type {typeof(NotFoundResult).Name}");

            //Creo la struttura di uscita
            return new ActionResultStructure<NotFoundResult, object>
            {
                Response = castedResponse,
                Data = null
            };
        }

        /// <summary>
        /// Parses provided response as "NotFound - 404"
        /// </summary>
        /// <param name="response">Response to parse</param>
        /// <returns>Returns parsed response</returns>
        protected ActionResultStructure<NotFoundObjectResult, object> ParseExpectedNotFoundObject(IActionResult response)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Mi attendo un 404
            if (!(response is NotFoundObjectResult castedResponse))
                throw new InvalidProgramException($"Response should be of type {typeof(NotFoundResult).Name}");

            //Creo la struttura di uscita
            return new ActionResultStructure<NotFoundObjectResult, object>
            {
                Response = castedResponse,
                Data = castedResponse.Value
            };
        }

        /// <summary>
        /// Parses provided response as "Forbid - 403"
        /// </summary>
        /// <param name="response">Response to parse</param>
        /// <returns>Returns parsed response</returns>
        protected ActionResultStructure<ForbidResult, object> ParseExpectedForbid(IActionResult response)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Mi attendo un 200
            if (!(response is ForbidResult castedResponse))
                throw new InvalidProgramException($"Response should be of type {typeof(ForbidResult).Name}");

            //Creo la struttura di uscita
            return new ActionResultStructure<ForbidResult, object>
            {
                Response = castedResponse,
                Data = null
            };
        }

        /// <summary>
        /// Parses provided response as "Forbid - 403"
        /// </summary>
        /// <param name="response">Response to parse</param>
        /// <returns>Returns parsed response</returns>
        protected ActionResultStructure<UnauthorizedResult, object> ParseExpectedUnauthorized(IActionResult response)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Mi attendo un 200
            if (!(response is UnauthorizedResult castedResponse))
                throw new InvalidProgramException($"Response should be of type {typeof(UnauthorizedResult).Name}");

            //Creo la struttura di uscita
            return new ActionResultStructure<UnauthorizedResult, object>
            {
                Response = castedResponse,
                Data = null
            };
        }

        /// <summary>
        /// Parses provided response as "BadRequest - 400"
        /// </summary>
        /// <param name="response">Response to parse</param>
        /// <returns></returns>
        protected ActionResultStructure<BadRequestObjectResult, SerializableError> ParseExpectedBadRequest(IActionResult response)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Mi attendo un 200
            if (!(response is BadRequestObjectResult castedResponse))
                throw new InvalidProgramException($"Response should be of type {typeof(BadRequestObjectResult).Name}");

            //Attendo che il risultato del tipo atteso
            if (!(castedResponse.Value is SerializableError castedResult))
                throw new InvalidProgramException($"Response data should be of type {typeof(SerializableError).FullName}");

            //Creo la struttura di uscita
            return new ActionResultStructure<BadRequestObjectResult, SerializableError>
            {
                Response = castedResponse,
                Data = castedResult
            };
        }

        /// <summary>
        /// Executes cleanup
        /// </summary>
        private void ExecuteCleanup()
        {
            //Cleanup del controller e scenario, layer
            //Controller?.Dispose();
            Controller = null;
            Scenario = null;
        }

        /// <summary>
        /// Overridable extra initialization
        /// </summary>
        protected virtual void OnCleanup()
        {
            //Default implementation is blank
        }

        /// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
        ~ApiControllerTestsBase()
        {
            //Richiamo i dispose implicito
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Eseguo una dispose esplicita
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">Explicit dispose</param>
        private void Dispose(bool isDisposing)
        {
            //Custom extra cleanup
            OnCleanup();

            //Executes base cleanup
            ExecuteCleanup();

            //Se l'oggetto è già rilasciato, esco
            if (_IsDisposed)
                return;

            //Se è richiesto il rilascio esplicito
            if (isDisposing)
            {
                //RIlascio della logica non finalizzabile                
            }

            //Marco il dispose e invoco il GC
            _IsDisposed = true;
        }
    }
}
