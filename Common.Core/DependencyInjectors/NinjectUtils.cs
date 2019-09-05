using System;
using Ninject;
using Ninject.Syntax;

namespace Common.Core.DependencyInjectors
{
    public class NinjectUtils
    {
        //Inizializzazione statica
        private static readonly Lazy<IKernel> _Kernel = new Lazy<IKernel>(() => new StandardKernel());


        //Espongo il metodo ed ottengo la sintassi per il bindind
        private static IBindingToSyntax<TInterface> GetBindingToSyntax<TInterface>() =>  //di destinazione per l'interfaccia passata
                    _Kernel.Value.Rebind<TInterface>();

        public enum Scope { Singleton, Threaded,
            Transient
        }

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
            var bindingToSyntax = GetBindingToSyntax<TInterface>();

            //Eseguo il binding della sintassi al target
            var bindingOnSyntax = bindingToSyntax.To<TImplementation>();

            //Applico la policy di singleton per la cache
            bindingOnSyntax.InSingletonScope();

        }

        /// <summary>
        /// Registers provided interface to a single static instance
        /// </summary>
        /// <typeparam name="TInterface">Type of interface</typeparam>
        /// <typeparam name="TImplementation">Type of implementation</typeparam>
        public static void RegisterInstance<TInterface, TImplementation>(TImplementation instance)
            where TImplementation : class, TInterface

        {
            RegisterInstance<TInterface, TImplementation>(() => instance, Scope.Singleton);
        }


        /// <summary>
        /// Registers provided interface to a function that returns the concrete implementation
        /// </summary>
        /// <typeparam name="TInterface">Type of interface</typeparam>
        /// <typeparam name="TImplementation">Type of implementation</typeparam>
        public static void RegisterInstance<TInterface, TImplementation>(Func<TImplementation> instanceFactory,
                                                                         Scope scope = Scope.Singleton)
            where TImplementation : class, TInterface

        {
            //Espongo il metodo ed ottengo la sintassi per il bindind
            //di destinazione per l'interfaccia passata
            var bindingToSyntax = GetBindingToSyntax<TInterface>();

            //Eseguo il binding della sintassi al target
            var bindingOnSyntax = bindingToSyntax.ToMethod((ctx) => instanceFactory());

            //Applico la policy di singleton per la cache
            switch(scope)
            {
                case Scope.Singleton:
                    bindingOnSyntax.InSingletonScope();
                    break;

                case Scope.Threaded:
                    bindingOnSyntax.InThreadScope();
                    break;

                case Scope.Transient:
                    bindingOnSyntax.InTransientScope();
                    break;
            }
        }

    }
}
