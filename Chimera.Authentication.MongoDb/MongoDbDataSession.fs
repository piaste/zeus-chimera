namespace Chimera.Authentication.MongoDb

open ZenProgramming.Chakra.Core.Data.Repositories
open ZenProgramming.Chakra.Core.Data.Repositories
open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open System.Linq.Expressions
open ZenProgramming.Chakra.Core.Data.Repositories
open ZenProgramming.Chakra.Core.Entities
open ZenProgramming.Chakra.Core.Data
open MongoDB.Driver
open Chakra.Core.MongoDb.Data.Repositories
open ZenProgramming.Chakra.Core.Data.Repositories.Helpers

type IMongoDbDataSession =
    inherit IDataSession

    abstract Database : IMongoDatabase

type MongoDbDataSession(options : MongoDbOptions) = 

    /// Public parameterless constructor for the DI framework
    new() = new MongoDbDataSession(MongoDbOptions.Default)

    /// Public options getter/setter for the DI framework
    member val Options = options

    interface IMongoDbDataSession with

        member this.Database = 
            
            let client = MongoClient(MongoUrl options.ConnectionString)
            client.GetDatabase options.Database

        member this.As<'TOutput>() = box this :?> 'TOutput

        member this.BeginTransaction() = raise (System.NotImplementedException())
        member this.Transaction = raise (System.NotImplementedException())

        member this.Dispose() = ()
        member this.ResolveRepository() = RepositoryHelper.Resolve this
        