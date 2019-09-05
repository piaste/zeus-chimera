namespace Chimera.Catalog.MongoDb

open Common.Providers.MongoDb
open Chimera.Catalog.Entities
open System.ComponentModel.DataAnnotations
open ZenProgramming.Chimera.Catalog.Data.Repositories

/// <summary> Stores Products in a MongoDB database</summary>
/// <param name="db">The MongoDB Database where this should be saved</param>
type MongoDbProductRepository(dataSession) = 

    inherit MongoDbRepository<Product>( dataSession
                                      , MongoDbProductRepository.ValidateProduct
                                      , "Products"
                                      )

  
    /// Validazione di esempio
    static member ValidateProduct category = 
        seq { if category.Name.Length >= 200000000 then yield ValidationResult "MongoDB is not web scale" }

    interface IProductRepository with
        
        member this.GetByCode(code) =
            base.AsRepository.Fetch(fun product -> product.Code = code)
            |> Seq.tryHead
            |> Option.defaultValue (null) // C# compatibility

