open System

let readInput =
    System.IO.File.ReadAllText(@"input/02.txt")

let has = 
    Map [("red", 12); ("green", 13); ("blue", 14)]

let solve (input: string) =
    input.Split("\n")
    |> Array.toSeq
    |> Seq.mapi (fun idx line ->
        let content = line.Split(": ").[1]
        let maxs = 
            content.Split("; ")
            |> Array.toSeq
            |> Seq.map (fun part -> 
                part.Split(", ")
                |> Array.toSeq
                |> Seq.map (fun s -> 
                    let (cnt, color) = s.Split(" ").[0], s.Split(" ").[1]
                    (color, int cnt)
                    )
                )
            |> Seq.concat
            |> Seq.groupBy (fun (color, cnt) -> color)
            |> Seq.map (fun (color, cnts) -> 
                let cnt = cnts |> Seq.maxBy (fun (_, cnt) -> cnt) |> snd
                (color, cnt)
                )

        let work = 
            maxs
            |> Seq.filter (fun (color, cnt) -> 
                    has[color] < cnt
                )
            |> Seq.isEmpty


        match work with
        | true -> idx + 1
        | _ -> 0
        

        )
    |> Seq.sum


let solve2 (input: string) =
    input.Split("\n")
    |> Array.toSeq
    |> Seq.mapi (fun idx line ->
        let content = line.Split(": ").[1]

        let maxs =
            content.Split("; ")
            |> Array.toSeq
            |> Seq.map (fun part ->
                part.Split(", ")
                |> Array.toSeq
                |> Seq.map (fun s ->
                    let (cnt, color) = s.Split(" ").[0], s.Split(" ").[1]
                    (color, int cnt)))
            |> Seq.concat
            |> Seq.groupBy (fun (color, cnt) -> color)
            |> Seq.map (fun (color, cnts) ->
                let cnt = cnts |> Seq.maxBy (fun (_, cnt) -> cnt) |> snd
                (color, cnt))

        maxs
        |> Seq.fold (fun acc (color, cnt) ->
            acc * cnt
            ) 1
        )

    |> Seq.sum

readInput
|> solve2
|> printfn "%d"