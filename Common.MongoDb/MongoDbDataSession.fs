namespace Common.Providers.MongoDb

open ZenProgramming.Chakra.Core.Data
open MongoDB.Driver
open ZenProgramming.Chakra.Core.Data.Repositories.Helpers
open Common.Core.DependencyInjectors
open ZenProgramming.Chakra.Core.Entities
open MongoDB.Driver
open System.Security


type MongoDbDataSession(options : MongoDbOptions) = 

    
    /// Helper function to generate a fake transaction in committed / uncommitted state.
    /// Transactions on Mongo are still WIP
    let fakeTransaction committed = {
        new IDataTransaction with
            member __.Commit() = ()
            member __.IsActive = not committed
            member __.Rollback() = ()
            member __.WasCommitted = committed
            member __.WasRolledBack = false
            member __.Dispose() = ()
    }

    /// Public parameterless constructor for the DI framework
    new() = new MongoDbDataSession(NinjectUtils.Resolve<MongoDbOptions>())
      

    /// Public options getter/setter for the DI framework
    member val Options = options


    interface IMongoDbDataSession with

        member this.Database = 
            
            let settings = 
                MongoClientSettings
                    ( Server = MongoServerAddress(this.Options.Host, if this.Options.Port.HasValue then this.Options.Port.Value else 27107)
                    , Credentials = [ MongoCredential.CreateCredential( this.Options.Database 
                                                                      , this.Options.Username
                                                                      , this.Options.Password
                                                                      )
                                    ]
                    )
            let client = MongoClient settings
            client.GetDatabase(this.Options.Database)

        member this.As<'TOutput when 'TOutput : not struct >() = box this :?> 'TOutput

        member this.BeginTransaction() = fakeTransaction false
            
        member this.Transaction = fakeTransaction true

        member this.Dispose() = ()
        member this.ResolveRepository() = 
            RepositoryHelper.Resolve<_, IMongoDbRepository> this
        