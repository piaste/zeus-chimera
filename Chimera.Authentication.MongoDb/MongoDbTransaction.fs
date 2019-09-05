/// DRAFT: implementazione delle transazioni in software
module internal Chimera.Authentication.MongoDb.Transactions

open MongoDB.Driver
open ZenProgramming.Chakra.Core.Entities
open System.Collections.Concurrent
open ZenProgramming.Chakra.Core.Data

type Operation<'T when 'T :> IEntity > = 
    | Create of newEntity: 'T
    | Update of oldEntity: 'T * newEntity: 'T
    | Delete of oldEntity: 'T
    | Commit



let cast<'T1, 'T2 when 'T1 :> IEntity and 'T2 :> IEntity> (x : Operation<'T1>)= 
    match x with
    | Create e -> Create (box e :?> 'T2)
    | Update (e1, e2) -> Update (box e1 :?> 'T2, box e2 :?> 'T2)
    | Delete e -> Create (box e :?> 'T2)
    | Commit -> Commit

type Older<'T> = Existing of 'T | Deleted of 'T

let show = 
    function
    | Commit -> None
    | Delete e -> Some (Deleted e)
    | Create e | Update(_, e) -> Some (Existing e) 

let showVal (Existing e | Deleted e) = e

let getId (x : #IEntity) = x.GetId().ToString()

let findLatestUpdate filter =     
    Seq.choose show
    >> Seq.filter (showVal >> filter)
    >> Seq.mapi (fun i e -> (i, e))
    >> Seq.groupBy (snd >> showVal >> getId)
    >> Map
    >> Map.map (fun _ v -> Seq.maxBy fst v |> snd)

let rec rollback (collection : IMongoCollection<'T>) find ops = 
    match ops with
    | [] -> []
    | latest :: previous -> 
        match latest with
        | Create t -> 
            collection.DeleteOne (find t) |> ignore
            rollback collection find previous

        | Update (oldEntity, newEntity) -> 
            collection.FindOneAndReplace(find newEntity, oldEntity) |> ignore
            rollback collection find previous

        | Delete oldEntity -> 
            collection.InsertOne(oldEntity)
            rollback collection find previous

        | Commit -> 
            // stop reverting
            ops 


type Transaction() =
    let mutable ops : (Operation<IEntity> * (unit -> unit)) list = []
    
    member this.Add (op, action) = ops <- (cast<_, IEntity> op, action) :: ops
       
    member this.Fetch<'T when 'T :> IEntity> filter = 
        findLatestUpdate filter [|            
            for op, _ in ops do
                match show op with
                | Some version when (showVal version :? 'T) -> yield cast<IEntity, 'T> op
                | _ -> ()
        |]


    interface IDataTransaction with
        member this.Commit() = 
            for (o, act) in ops do
                try act()
                with e -> this.Add (Commit, ignore)


        member this.Dispose() = ()
        member this.IsActive = true
        member this.Rollback() =  ()
            //for (o, act) in ops do
        member this.WasCommitted = raise (System.NotImplementedException())
        member this.WasRolledBack = raise (System.NotImplementedException()) 

