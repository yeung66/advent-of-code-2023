open System.IO

let readInput = File.ReadAllText(@"input/05.txt")

let seed2Map (seeds: int64 seq) (map: (int64 * int64 * int64) array) =
    seeds
    |> Seq.map (fun seed ->
        let mutable toIdx = seed

        for (target, source, length) in map do
            if seed < source + length && seed >= source then
                let offset = seed - source
                toIdx <- target + offset

        toIdx)

let part1 (input: string) =
    let maps = input.Split("\n\n")
    let seeds = maps.[0].Substring(7).Split(" ") |> Array.map int64

    let maps =
        maps.[1..]
        |> Array.map (fun m ->
            m.Split("\n").[1..]
            |> Array.map (fun line ->
                let nums = line.Split(" ") |> Array.map int64
                (nums.[0], nums.[1], nums.[2])

            ))

    maps |> Array.fold (fun seeds map -> seed2Map seeds map) seeds |> Seq.min


let part2 (input: string) =
    let maps = input.Split("\n\n")
    let seeds = maps.[0].Substring(7).Split(" ") |> Array.map int64

    let seeds =
        [| for i in 0 .. seeds.Length - 1 do
               if i % 2 = 0 then
                   yield (seeds.[i], seeds[i + 1]) |]
        |> Array.map (fun (a, b) ->
            printfn "%d %d = %d" a b (a + b)
            (a, b)
        )
        |> Array.toSeq

        |> Seq.collect (fun (a, b) ->
            [| for i in a .. (a + b - (int64 1)) do
                   yield i |])

    // seeds.Length |> printfn "%d"

    let maps =
        maps.[1..]
        |> Array.map (fun m ->
            m.Split("\n").[1..]
            |> Array.map (fun line ->
                let nums = line.Split(" ") |> Array.map int64
                (nums.[0], nums.[1], nums.[2])

            ))

    maps 
    |> Array.fold (fun seeds map -> seed2Map seeds map) seeds 
    |> Seq.min

let part2Async (input: string) = 
    let maps = input.Split("\n\n")
    let seeds = maps.[0].Substring(7).Split(" ") |> Array.map int64

    let maps =
        maps.[1..]
        |> Array.map (fun m ->
            m.Split("\n").[1..]
            |> Array.map (fun line ->
                let nums = line.Split(" ") |> Array.map int64
                (nums.[0], nums.[1], nums.[2])

            ))

    let seeds =
        [| for i in 0 .. seeds.Length - 1 do
               if i % 2 = 0 then
                   yield (seeds.[i], seeds[i + 1]) |]
        |> Array.map (fun (a, b) ->
            printfn "%d %d = %d" a b (a + b)
            (a, b)
        )
        |> Array.toSeq

    seeds
    |> Seq.map (fun (a, b) ->
        async {
            let allSeeds = 
                [| for i in a .. (a + b - (int64 1)) do
                        yield i |]

            return maps |> Array.fold (fun seeds map -> seed2Map seeds map) allSeeds |> Seq.min

        })
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Array.min

readInput |> part1 |> printfn "Part 1: %d"
readInput |> part2Async |> printfn "Part 2: %d"