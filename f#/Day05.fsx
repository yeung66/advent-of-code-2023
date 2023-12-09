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
            (a, b))
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

    maps |> Array.fold (fun seeds map -> seed2Map seeds map) seeds |> Seq.min

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
            (a, b))
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

type Range = { start: int64; endd: int64 }

let intersect (range1: Range) (range2: Range) : Range option =
    // Implement intersect logic here
    if range1.endd <= range2.start || range2.endd <= range1.start then
        None
    else
        let start = max range1.start range2.start
        let endd = min range1.endd range2.endd
        Some { start = start; endd = endd }

let difference (range1: Range) (range2: Range) : (Range option * Range option) =
    // Implement difference logic here
    let leftDiff =
        if range1.start < min range2.start range1.endd then
            Some
                { start = range1.start
                  endd = min range2.start range1.endd }
        else
            None

    let rightDiff =
        if range1.endd > max range2.endd range1.start then
            Some
                { start = max range2.endd range1.start
                  endd = range1.endd }
        else
            None

    (leftDiff, rightDiff)


let mapRanges (seeds: Range list) (maps: (int64 * int64 * int64) list list) : Range list =
    let rec processMaps (ranges: Range list) (maps: (int64 * int64 * int64) list list) : Range list =
        match maps with
        | [] -> ranges
        | map :: restMaps ->
            let newRanges = processMap ranges map
            processMaps newRanges restMaps

    and processMap (ranges: Range list) (map: (int64 * int64 * int64) list) : Range list =
        ranges |> List.collect (fun range -> processRange range map)

    and processRange (range: Range) (map: (int64 * int64 * int64) list) : Range list =
        match map with
        | [] -> [ range ]
        | (target, source, len) :: restMap ->
            let mappedRange = { start = source; endd = source + len }

            match intersect range mappedRange with
            | None -> processRange range restMap
            | Some intersection ->
                let newRange =
                    { start = intersection.start - source + target
                      endd = intersection.endd - source + target }

                let (leftDiff, rightDiff) = difference range mappedRange
                let extraRanges = [ leftDiff; rightDiff ] |> List.choose id
                newRange :: (extraRanges @ processMap extraRanges restMap)

    processMaps seeds maps

let part2Better (input: string) =
    let maps = input.Split("\n\n")
    let seeds = maps.[0].Substring(7).Split(" ") |> Array.map int64

    let maps =
        maps.[1..]
        |> Array.map (fun m ->
            m.Split("\n").[1..]
            |> Array.map (fun line ->
                let nums = line.Split(" ") |> Array.map int64
                (nums.[0], nums.[1], nums.[2])

            )
            |> Array.toList)
        |> Array.toList

    let seeds =
        [ for i in 0 .. seeds.Length - 1 do
              if i % 2 = 0 then
                  yield
                      { start = seeds.[i]
                        endd = seeds.[i] + seeds.[i + 1] } ]


    mapRanges seeds maps
    |> List.sortBy (fun range -> range.start)
    |> List.rev
    |> List.map (fun range ->
        printfn "%d %d " range.start range.endd
        range.start)
    |> List.min




// readInput |> part1 |> printfn "Part 1: %d"
// readInput |> part2Async |> printfn "Part 2: %d"

readInput |> part2Better |> printfn "Part 2: %d"
