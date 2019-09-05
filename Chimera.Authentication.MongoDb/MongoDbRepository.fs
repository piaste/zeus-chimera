namespace Chimera.Authentication.MongoDb

open ZenProgramming.Chakra.Core.Data.Repositories
open ZenProgramming.Chakra.Core.Data.Repositories
open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open System.Linq.Expressions
open ZenProgramming.Chakra.Core.Data.Repositories
open ZenProgramming.Chakra.Core.Entities
open MongoDB.Driver
open Chakra.Core.MongoDb.Data.Repositories
open Chimera.Authentication.MongoDb.Transactions
open ZenProgramming.Chakra.Core.Data
open MongoDB.Bson.IO


/// <summary>
/// Stores an entity inside a MongoDb database.
/// <summary>
type MongoDbRepository<'TEntity  
                        when 'TEntity : (new : unit -> 'TEntity)
                         and 'TEntity : not struct 
                         and 'TEntity :> IEntity 
                         and 'TEntity : null
                    >

           /// <summary> Internal constructor </summary>
           /// <param name="dataSession">A data session to an existing MongoDB database</param>
           /// <param name="validationRules">Apply a function to the entity to verify any MongoDb-specific validation rules (eg. data size limits)</param>
           /// <param name="collectionName">Customize the collection name. The default is the entity type's name.</param>
           internal ( dataSession : IMongoDbDataSession                    
                    , validationRules : 'TEntity -> ValidationResult seq
                    , ?collectionName : string
                    ) = 
        
    

    let db = dataSession.Database

    /// If not specified, catalog name is the type name
    let collectionName = collectionName |> Option.defaultValue (typeof<'TEntity>.Name)
    
    let entities () = db.GetCollection(collectionName)
        
    /// TODO : test the fake transaction implementation
    let transaction : Transaction option = None
        
    /// Cast to IRepository
    member this.AsRepository = (this :> IRepository<'TEntity>)

    interface IRepository<'TEntity> with
        
        member this.Count(filterExpression) = 
            let longCount = entities().CountDocuments(filterExpression)
            if longCount > (int64 Int32.MaxValue) then -1 else int longCount

        member this.Delete(entity) = 
            entities().DeleteOne(fun (e : 'TEntity) -> entity.GetId() = e.GetId())
            |> ignore

        member this.Dispose() = 
            transaction |> Option.iter (fun t -> (t :> IDataTransaction).Rollback())

        member this.Fetch(filterExpression, startRowIndex, maximumRows, sortExpression, isDescending) = 

            let filteredEntities = entities().Find(filterExpression)
                    
            let sort things = 
                if isDescending then IFindFluentExtensions.SortByDescending<_, _>(things, sortExpression) 
                                else IFindFluentExtensions.SortBy<_, _>(things, sortExpression)

            let sortedEntities = sort filteredEntities
            
            let boundedEntities = sortedEntities.Skip(startRowIndex).Limit(maximumRows)

            boundedEntities.ToList() :> IList<_>
            

        member this.GetSingle(expression) = 
            (this :> IRepository<_>).Fetch(expression, Nullable 0 , Nullable 1, (fun _ -> null), false)
            |> Seq.tryHead
            |> Option.defaultValue null

        member this.IsValid(entity) =
            (this :> IRepository<_>).Validate(entity) |> Seq.isEmpty

        member this.Save(entity) = 
            entities().InsertOne(entity)

        member this.Validate(entity) = 
            let mutable results = ResizeArray<_>()
            let success = Validator.TryValidateObject(entity, ValidationContext entity, results)

            results.AddRange(validationRules(entity))

            results :> IList<_>

    