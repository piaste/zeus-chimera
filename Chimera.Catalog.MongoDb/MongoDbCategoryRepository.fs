namespace Chimera.Catalog.MongoDb

open Common.Providers.MongoDb
open Chimera.Catalog.Entities
open System.ComponentModel.DataAnnotations
open ZenProgramming.Chimera.Catalog.Data.Repositories
open System.Collections.Generic

/// <summary> Stores Categorys in a MongoDB database</summary>
/// <param name="db">The MongoDB Database where this should be saved</param>
type MongoDbCategoryRepository(dataSession) = 

    inherit MongoDbRepository<Category>( dataSession
                                   , MongoDbCategoryRepository.ValidateCategory
                                   , "Categories"
                                   )

  
    /// Validazione di esempio
    static member ValidateCategory category = 
        seq { if category.Name.Length >= 200000000 then yield ValidationResult "MongoDB is not web scale" }

    interface ICategoryRepository with
        
        member this.GetByCode(code) =
            base.AsRepository.Fetch(fun category -> category.Code = code)
            |> Seq.tryHead
            |> Option.defaultValue (null) // C# compatibility


        member this.DeleteCategory (category) = 
            base.AsRepository.Delete (category)
            //return validation results after delete?
            ResizeArray<_>() :> IList<_>
    