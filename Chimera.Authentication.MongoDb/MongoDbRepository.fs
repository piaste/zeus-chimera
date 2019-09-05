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


/// Stores an entity inside a MongoDb database.
///
/// By default the collection name is the same as the type name.
type MongoDbRepository<'TEntity  
                        when 'TEntity : (new : unit -> 'TEntity)
                         and 'TEntity : not struct 
                         and 'TEntity :> IEntity 
                         and 'TEntity : null
                    >

           internal ( dataSession : IMongoDbDataSession                    
                    , ?validationRules : 'TEntity -> ValidationResult seq
                    , ?collectionName : string
                    ) = 
        
    

    
    // If not specified, validation always passes
    let validationRules = validationRules |> Option.defaultValue (fun _ -> Seq.empty)

    // If not specified, catalog name is the type name
    let collectionName = collectionName |> Option.defaultValue (typeof<'TEntity>.Name)
    
    // Open the database and collection
    let db = dataSession.Database    
    let entities () = db.GetCollection(collectionName)
        
    // TODO : test the fake transaction implementation
    let transaction : Transaction option = None
        
    

    /// Casts this to IRepository
    member this.AsRepository = (this :> IRepository<'TEntity>)




    // Implement the repo pattern
    interface IRepository<'TEntity> with
        
        member this.Count(filterExpression) = 
            let longCount = entities().CountDocuments(filterExpression)
            if longCount > (int64 Int32.MaxValue) then -1 else int longCount

        member this.Delete(entity) = 
            entities().DeleteOne(fun (e : 'TEntity) -> entity.GetId() = e.GetId())
            |> ignore
                    
        member this.Dispose() = 
            // By default rolls back transaction if not committed
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

            /// Apply the entity's own rules
            let success = Validator.TryValidateObject(entity, ValidationContext entity, results)

            /// Add the repo-specific rules
            results.AddRange(validationRules(entity))

            results :> IList<_>

    