namespace Common.Providers.MongoDb

open System

[<CLIMutable>]
type MongoDbOptions = {

    Username: string
    Password: string
    Host: string

    /// Default: 27017
    Port: Nullable<int>

    Database : string
}
with
  /// Default: connects to localhost and opens "database"
  static member Default = 
    {   Username = "" 
        Password = ""
        Host = "localhost"
        Port = Nullable<_>()
        Database = "database"
    }

  member this.ConnectionString = 
    let u = UriBuilder()
    u.Scheme <- "mongodb"
    u.UserName <- this.Username
    u.Password <- this.Password
    u.Host <- this.Host
    u.Port <- this.Port.GetValueOrDefault 27107
    u.ToString()

