namespace Common.Providers.MongoDb

open ZenProgramming.Chakra.Core.Data
open MongoDB.Driver
open ZenProgramming.Chakra.Core.Data.Repositories.Helpers
open ZenProgramming.Chimera.Common.Contracts.DependencyInjectors

type IMongoDbDataSession =
    inherit IDataSession

    abstract Database : IMongoDatabase

type MongoDbDataSession(options : MongoDbOptions) = 

    /// Public parameterless constructor for the DI framework
    new() = new MongoDbDataSession(NinjectUtils.Resolve<MongoDbOptions>())

    /// Public options getter/setter for the DI framework
    member val Options = options

    interface IMongoDbDataSession with

        member this.Database = 
            
            let client = MongoClient(MongoUrl this.Options.ConnectionString)
            client.GetDatabase this.Options.Database

        member this.As<'TOutput when 'TOutput : not struct >() = box this :?> 'TOutput

        member this.BeginTransaction() = raise (System.NotImplementedException())
        member this.Transaction = raise (System.NotImplementedException())

        member this.Dispose() = ()
        member this.ResolveRepository() = RepositoryHelper.Resolve this
        