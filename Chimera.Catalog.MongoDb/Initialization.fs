﻿module Chimera.Catalog.MongoDb.Initialization

open Chimera.Catalog.Settings
open System
open Common.Providers.MongoDb



/// Read all MongoDb options from settings
let ReadOptions (settings : CatalogSettings) = 

    let parse (connString : ConnectionStringSettings) =
        try 
            Ok {
                Host = connString.Url
                Port = Nullable 27017
                Username = connString.Username
                Password = connString.Password
                Database = "chimera-catalog" // Should this be in settings?
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