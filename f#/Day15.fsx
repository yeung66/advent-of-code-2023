
let input = System.IO.File.ReadAllText "input/15.txt"

let hash (content: string) = 
    content.ToCharArray() |> Seq.fold (fun acc c -> (acc +  (c |> int)) * 17 % 256) 0


let part1 () =
    input.Split(",") |> Array.map hash |> Array.sum


type Lens = { label: string; focal: int}

type Operation = 
    | Dash of string
    | Equal of string * int

type Boxes = Lens list list
    
let parseOperation (op: string) = 
    if op.Contains("-") then
        Dash (op.Substring(0, op.Length - 1))
    else
        let equal = op.Split("=").[1] |> int
        let label = op.Split("=").[0]
        Equal (label, equal)

let removeAt index list =
    list
    |> List.mapi (fun i x -> i, x)
    |> List.filter (fun (i, _) -> i <> index)
    |> List.map snd

let replaceAt index value list =
    list
    |> List.mapi (fun i x -> i, x)
    |> List.map (fun (i, x) -> if i = index then value else x)

let part2 () =
    let boxes: Boxes = [0..255] |> List.map (fun i -> [])
    let operations = input.Split(",") |> Array.map parseOperation
    operations |> Array.fold (fun (boxes: Boxes) op -> 
        match op with
        | Dash label -> 
            let h = hash label
            let box = boxes.[h]
            match box |> List.tryFindIndex (fun l -> l.label = label) with
            | Some idx ->
                replaceAt h (removeAt idx box) boxes
            | None -> boxes
        | Equal (label, equal) ->
            let h = hash label
            let box = boxes.[h]
            match box |> List.tryFindIndex (fun l -> l.label = label) with
            | Some idx ->
                let newBox = replaceAt idx { label = label; focal = equal } box
                replaceAt h newBox boxes
                
            | None ->
                let newBox = box @ [{ label = label; focal = equal }]
                replaceAt h newBox boxes

    ) boxes

    

    |> Seq.mapi (fun i1 box -> 
        box |> Seq.mapi (fun i2 lens -> 
            (i1 + 1) * (i2 + 1) * lens.focal
        ) |> Seq.sum)
    |> Seq.sum


part1 () |> printfn "part1: %d"
part2 () |> printfn "part2: %d"
