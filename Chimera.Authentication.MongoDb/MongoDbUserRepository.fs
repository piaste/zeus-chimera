namespace Chimera.Authentication.MongoDb

open Common.Providers.MongoDb
open Chimera.Authentication.Entities
open System.ComponentModel.DataAnnotations
open Chimera.Authentication.Data.Repositories
open ZenProgramming.Chakra.Core.Data.Repositories.Attributes

/// <summary> Stores users in a MongoDB database</summary>
/// <param name="db">The MongoDB Database where this should be saved</param>
type [<Repository>] MongoDbUserRepository(dataSession) = 

    inherit MongoDbRepository<User>( dataSession
                                   , MongoDbUserRepository.ValidateUser
                                   , "Users"
                                   )

  
    /// Validazione di esempio
    static member ValidateUser user = 
        seq { if user.UserName.Length >= 200000000 then yield ValidationResult "MongoDB is not web scale" }

    interface IUserRepository with
        
        member this.GetUserByUserName(userName) =
            base.AsRepository.Fetch(fun user -> user.UserName = userName)
            |> Seq.tryHead
            |> Option.defaultValue (null) // C# compatibilityy
    