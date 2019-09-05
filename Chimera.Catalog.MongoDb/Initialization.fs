module Chimera.Catalog.MongoDb.Initialization

open Chimera.Catalog.Settings
open System
open Common.Providers.MongoDb



/// Read all MongoDb options from settings
let ReadOptions (settings : CatalogSettings) = 

    let parse (connString : ConnectionStringSettings) =
        try 
            let uri = Uri connString.Url
    
            Ok {
                Host = uri.Host 
                Port = Nullable uri.Port
                Username = connString.Username
                Password = connString.Password
                Database = "Catalog"               
            }

        with ex -> Error ex

    dict [ 
        if String.Equals(settings.Storage.ProviderName, "Mongo", StringComparison.InvariantCultureIgnoreCase) then

            for connString in settings.ConnectionStrings do
                match parse connString with
                | Ok options ->
                    // Correctly configured
                    yield connString.Name, options
                | Error uriException ->
                    // Incorrectly configured
                    // Log the exception?
                    raise uriException
    ]