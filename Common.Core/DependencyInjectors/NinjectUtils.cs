using System;
using Ninject;

namespace Common.Core.DependencyInjectors
{
    public class NinjectUtils
    {
        //Inizializzazione statica
        private static readonly Lazy<IKernel> _Kernel = new Lazy<IKernel>(() => new StandardKernel());

        /// <summary>
        /// Resolve specified interface
        /// </summary>
        /// <typeparam name="TInterface">Type of interface</typeparam>
        /// <returns>Returns resolution</returns>
        public static TInterface Resolve<TInterface>()
        {
            //Ritorno la risoluzione
            return _Kernel.Value.Get<TInterface>();
        }

        /// <summary>
        /// Registers provided interface to concrete type
        /// </summary>
        /// <typeparam name="TInterface">Type of interface</typeparam>
        /// <typeparam name="TImplementation">Type of implementation</typeparam>
        public static void Register<TInterface, TImplementation>()
            where TImplementation: class, TInterface
        {
            //Espongo il metodo ed ottengo la sintassi per il bindind
            //di destinazione per l'interfaccia passata
            var bindingToSyntax =_Kernel.Value.Rebind<TInterface>();

            //Eseguo il binding della sintassi al target
            var bindingOnSyntax = bindingToSyntax.To<TImplementation>();

            //Applico la policy di singleton per la cache
            bindingOnSyntax.InSingletonScope();
        }
    }
}
