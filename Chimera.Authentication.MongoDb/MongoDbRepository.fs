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



type MongoDbRepository<'TEntity  
                        when 'TEntity : (new : unit -> 'TEntity)
                         and 'TEntity : not struct 
                         and 'TEntity :> IEntity 
                         and 'TEntity : null
                    >

                    /// Constructor
                    ( dataSession : IMongoDbDataSession
                    , repoValidation : 'TEntity -> ValidationResult seq
                    , ?entityName : string
                    // TODO: completare
                    // , ?transaction : Transaction
                    ) = 
    

    // "mongodb:///zeus-mongo-01.japaneast.cloudapp.azure.com:27017"
    // "chimera-catalog"
    // "password"

    let transaction : Transaction option = None
    
    let db = dataSession.Database

    let entityName = entityName |> Option.defaultValue (typeof<'TEntity>.Name)
    
    let entities () = db.GetCollection(entityName)
        

    member this.AsRepository = (this :> IRepository<'TEntity>)

    interface IRepository<'TEntity> with
        
        member this.Count(filterExpression) = 
            let longCount : int64 = entities().CountDocuments(filterExpression)
            if longCount > (int64 Int32.MaxValue) then -1 else int longCount

        member this.Delete(entity) = 
            let action() = 
                entities().DeleteOne(fun (e : 'TEntity) -> entity.GetId() = e.GetId())
                |> ignore
            
            match transaction with
            | None -> action() 
            | Some t -> t.Add(Delete entity, action)
                

        member this.Dispose() = 
            transaction |> Option.iter (fun t -> (t :> IDataTransaction).Rollback())


        member this.Fetch(filterExpression, startRowIndex, maximumRows, sortExpression, isDescending) = 


            let filteredEntities = 
                entities()
                    .Find(filterExpression)
                    
            let sort things = 
                if isDescending then IFindFluentExtensions.SortByDescending<_, _>(things, sortExpression) 
                                else IFindFluentExtensions.SortBy<_, _>(things, sortExpression)

            let sortedEntities = sort filteredEntities

                        

            let boundedEntities = 
                sortedEntities
                    .Skip(startRowIndex)
                    .Limit(maximumRows)
                    .ToList()

            match transaction with
            | None -> boundedEntities
            | Some t -> 
                let transactionUpdates = t.Fetch (filterExpression.Compile().Invoke)
                
                if Map.isEmpty transactionUpdates 
                    then boundedEntities
                    else ResizeArray [|
                        for entity in boundedEntities do
                            match Map.tryFind (getId entity) transactionUpdates with
                            | Some (Existing entityUpdate)  -> yield entityUpdate
                            | Some (Deleted _) -> ()
                            | None -> yield entity
                    |] // TODO: sort again

            :> IList<_>
            

        member this.GetSingle(expression) = 
            (this :> IRepository<_>).Fetch(expression, Nullable 0 , Nullable 1, (fun _ -> null), false)
            |> Seq.tryHead
            |> Option.defaultValue null

        member this.IsValid(entity) =
            (this :> IRepository<_>).Validate(entity) |> Seq.isEmpty

        member this.Save(entity) = 
            let action() = 
                entities().InsertOne(entity)
        
            match transaction with
            | None -> action() 
            | Some t -> t.Add(Create entity, action)

        member this.Validate(entity) = 
            let mutable results = ResizeArray<_>()
            let success = Validator.TryValidateObject(entity, ValidationContext entity, results)

            results.AddRange(repoValidation(entity))

            results :> IList<_>

    