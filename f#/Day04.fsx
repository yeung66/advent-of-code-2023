
open System

let readInput =
    System.IO.File.ReadAllText(@"input/04.txt")

let rec pow2 n =
    if n = 0 then 1
    else 2 * pow2 (n - 1)

let parse (line: string) = 
    let line = line.Split(": ").[1]
    
    line.Split(" | ")
    |> Array.map (fun part -> 
        part.Split(" ")
        |> Array.filter (fun num -> num <> "")
        |> Array.map (fun num -> int num)
        |> Set.ofArray
        )
    
            



let part1 (input: string) = 
    input.Split("\n")
    |> Array.map (fun line ->
        let nums = parse line
        let (winning, have) = (nums.[0], nums.[1])
        
        let size = have 
                |> Set.filter (fun num -> Set.contains num winning)
                |> Set.count
        
        match size with
        | 0 -> 0
        | i -> pow2 (i - 1)

    )
    |> Array.sum
                
let part2 (input: string) = 
    let mutable cardCount = 
        [| for i in 0..input.Split("\n").Length - 1 do 
            yield 1  |]

    input.Split("\n")
    |> Array.mapi (fun idx line ->
        let nums = parse line
        let (winning, have) = (nums.[0], nums.[1])
        
        let win = have 
                |> Set.filter (fun num -> Set.contains num winning)
                |> Set.count

        for i in 1..win do
            cardCount.[idx + i] <- cardCount.[idx + i] + cardCount.[idx]
        
        cardCount.[idx]
    )
    |> Array.sum


readInput |> part1 |> printfn "%d"
readInput |> part2 |> printfn "%d"