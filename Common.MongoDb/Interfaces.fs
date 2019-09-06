namespace Common.Providers.MongoDb

open ZenProgramming.Chakra.Core.Data.Repositories
open ZenProgramming.Chakra.Core.Entities
open ZenProgramming.Chakra.Core.Data
open MongoDB.Driver


/// Stores an entity inside a MongoDb database.
///
/// By default the collection name is the same as the type name.
type IMongoDbRepository = inherit IRepository
      
            
/// Opens a data session with a MongoDB database
type IMongoDbDataSession =
    inherit IDataSession
      
    abstract Database : IMongoDatabase