open System.IO

let readInput = File.ReadAllText "input/07.txt"


type HandType =
    | FIVE of int array
    | FOUR of int array
    | FULLHOUSE of int array
    | THREE of int array
    | TWOPAIR of int array
    | PAIR of int array
    | HIGHCARD of int array

let handTypePriority (handType: HandType) =
    match handType with
    | FIVE _ -> 8
    | FOUR _ -> 7
    | FULLHOUSE _ -> 6
    | THREE _ -> 5
    | TWOPAIR _ -> 4
    | PAIR _ -> 3
    | HIGHCARD _ -> 2


let compareHandType (a: HandType) (b: HandType) =
    let aPriority = handTypePriority a
    let bPriority = handTypePriority b

    if aPriority <> bPriority then
        compare aPriority bPriority
    else
        match (a, b) with
        | (FIVE a, FIVE b) -> compare a b
        | (FOUR a, FOUR b) -> compare a b
        | (FULLHOUSE a, FULLHOUSE b) -> compare a b
        | (THREE a, THREE b) -> compare a b
        | (TWOPAIR a, TWOPAIR b) -> compare a b
        | (PAIR a, PAIR b) -> compare a b
        | (HIGHCARD a, HIGHCARD b) -> compare a b
        | _ -> 0

let handType (hand: int array) =
    let counts =
        hand |> Array.groupBy id |> Array.map (fun (k, v) -> v.Length) |> Array.sort

    match counts with
    | [| 5 |] -> FIVE hand
    | [| 1; 4 |] -> FOUR hand
    | [| 2; 3 |] -> FULLHOUSE hand
    | [| 1; 1; 3 |] -> THREE hand
    | [| 1; 2; 2 |] -> TWOPAIR hand
    | [| 1; 1; 1; 2 |] -> PAIR hand
    | _ -> HIGHCARD hand

let getCard (card: string) =
    card.ToCharArray()
    |> Array.map (fun c ->
        match c with
        | 'T' -> 10
        | 'J' -> 11
        | 'Q' -> 12
        | 'K' -> 13
        | 'A' -> 14
        | _ -> int c - int '0')


let part1 (input: string) =
    input.Split("\n")
    |> Array.map (fun line ->
        let cards = line.Substring(0, 5)
        let bid = line.Substring(6)

        (cards |> getCard |> handType, int bid))
    |> Array.sortWith (fun (a, _) (b, _) -> compareHandType a b)
    |> Array.mapi (fun i (hand, bid) ->
        // printfn "%d %d %A" (i+1) bid hand
        bid * (i + 1))
    |> Array.sum

let getCard2 (card: string) =
    card.ToCharArray()
    |> Array.map (fun c ->
        match c with
        | 'T' -> 10
        | 'J' -> 0
        | 'Q' -> 12
        | 'K' -> 13
        | 'A' -> 14
        | _ -> int c - int '0')

let handType2 (hand: int array) =
    let counts =
        if hand |> Array.exists (fun c -> c = 0) && hand |> Array.exists (fun c -> c <> 0) then
            let mostCard =
                hand
                |> Array.groupBy id
                |> Array.filter (fun (k, v) -> k <> 0)
                |> Array.sortBy (fun (k, v) -> v.Length)
                |> Array.last
                |> fst

            hand
            |> Array.map (fun c -> if c = 0 then mostCard else c)
            |> Array.groupBy id
            |> Array.map (fun (k, v) -> v.Length)
            |> Array.sort
        else
            hand |> Array.groupBy id |> Array.map (fun (k, v) -> v.Length) |> Array.sort


    match counts with
    | [| 5 |] -> FIVE hand
    | [| 1; 4 |] -> FOUR hand
    | [| 2; 3 |] -> FULLHOUSE hand
    | [| 1; 1; 3 |] -> THREE hand
    | [| 1; 2; 2 |] -> TWOPAIR hand
    | [| 1; 1; 1; 2 |] -> PAIR hand
    | _ -> HIGHCARD hand

let part2 (input: string) =
    input.Split("\n")
    |> Array.map (fun line ->
        let cards = line.Substring(0, 5)
        let bid = line.Substring(6)

        (cards |> getCard2 |> handType2, int bid))
    |> Array.sortWith (fun (a, _) (b, _) -> compareHandType a b)
    |> Array.mapi (fun i (hand, bid) ->
        // printfn "%d %d %A" (i + 1) bid hand
        bid * (i + 1))
    |> Array.sum

readInput |> part1 |> printfn "%d"

readInput |> part2 |> printfn "%d"
