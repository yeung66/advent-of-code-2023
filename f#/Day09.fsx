open System.IO
let readInput = File.ReadAllText(@"input/09.txt")

let predictSeq (nums: int64 list) getLast = 
    let genDiffSeq (nums: int64 list) = 
        nums
        |> List.mapi (fun i num -> 
            if i = 0 then
                (0L)
            else
                (num - nums.[i - 1]))
        |> List.tail

    let rec genNextDiffLastVal (nums: int64 list) = 
        let diffSeq = genDiffSeq nums
        // printfn "%A" diffSeq
        if diffSeq |> List.groupBy id |> List.length = 1 then
            diffSeq |> List.last
        else
            genNextDiffLastVal diffSeq + (diffSeq |> List.last)

    let rec genNextDiffFirstVal (nums: int64 list) = 
        let diffSeq = genDiffSeq nums
        // printfn "%A" diffSeq
        if diffSeq |> List.groupBy id |> List.length = 1 then
            diffSeq |> List.head
        else
            (diffSeq |> List.head) - (genNextDiffFirstVal diffSeq) 

    // printfn ""

    if getLast then
        genNextDiffLastVal nums + (nums |> List.last)
    else
        (nums |> List.head) - (genNextDiffFirstVal nums) 
let part1 (input: string) =
    input.Split("\n")
    |> Array.map (fun line -> 
        line.Split(" ") |> Array.map int64 |> Array.toList
        )
    |> Array.map (fun num -> 
        predictSeq num true
        )
    |> Array.sum

let part2 (input: string) =
    input.Split("\n")
    |> Array.map (fun line -> 
        line.Split(" ") |> Array.map int64 |> Array.toList
        )
    |> Array.map (fun num -> 
        predictSeq num false
        )
    |> Array.sum

readInput |> part1 |> printfn "%d"
readInput |> part2 |> printfn "%d"