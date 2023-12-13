open System.IO
let input = File.ReadAllText(@"input/13.txt")

type SymmetricLine = 
    | Horizontal
    | Vertical

let diffString (s1: char list) (s2: char list) = 
    // find how many characters are different between two strings
    let rec diffStringRec (s1: char list) (s2: char list) (i: int) (diff: int) =
        if i = s1.Length then
            diff
        else
            if s1.[i] = s2.[i] then
                diffStringRec s1 s2 (i + 1) diff
            else
                diffStringRec s1 s2 (i + 1) (diff + 1)
    diffStringRec s1 s2 0 0

let checkSymmetric (block: char list list) (line: int) (symmetricLine: SymmetricLine) (change: int) =
    let checkHorizontalSymmetric (block: char list list) (line: int) (change: int)=
        let rec checkSymmetricRec (i: int) (j: int) (change: int) =
            if i < 0 || j >= block.Length then
                change = 0
            else
                if block.[i] = block.[j] then
                    checkSymmetricRec (i - 1) (j + 1) change
                else
                    if change > 0 && (diffString block[i] block[j]) = 1 then
                        checkSymmetricRec (i - 1) (j + 1) (change - 1)
                    else
                        false

        checkSymmetricRec (line) (line + 1) change

    let checkVerticalSymmetric (block: char list list) (line: int) (change: int) =
        let columns =
            [ 0 .. block.[0].Length - 1 ]
            |> List.map (fun col -> block |> List.map (fun row -> row.[col]))

        let rec checkSymmetricRec (i: int) (j: int) (change: int) =
            if i < 0 || j = columns.Length then
                change = 0
            else
                if columns.[i] = columns.[j] then
                    checkSymmetricRec (i - 1) (j + 1) change
                else
                    if change > 0 && (diffString columns[i] columns[j]) = 1 then
                        checkSymmetricRec (i - 1) (j + 1) (change - 1)
                    else
                        false

        checkSymmetricRec (line) (line + 1) change

    match symmetricLine with
    | Vertical -> checkVerticalSymmetric block line change
    | Horizontal -> checkHorizontalSymmetric block line change

let solve (block: char list list) (change: int)= 
    let h =
        [ 0 .. block.Length - 2 ]
        |> List.filter (fun line -> checkSymmetric block line Horizontal change)

    if h.Length = 1 then
        // printfn "%A" block
        (h[0] + 1) * 100
    else
        let v =
            [ 0 .. block.[0].Length - 2 ]
            |> List.filter (fun line -> checkSymmetric block line Vertical change)
        if v.Length = 1 then
            v[0] + 1
        else
            0

let part1 () =
    input.Split("\n\n")
    |> Array.filter (fun block -> block <> "")
    |> Array.map (fun block ->
        block.Split("\n")
        |> Array.map (fun line -> line.ToCharArray())
        |> Array.toList
        |> List.map (fun x -> x |> Array.toList)

    )
    |> Array.map (fun block -> solve block 0
        )
    |> Array.sum

let part2 () =
    input.Split("\n\n")
    |> Array.filter (fun block -> block <> "")
    |> Array.map (fun block ->
        block.Split("\n")
        |> Array.map (fun line -> line.ToCharArray())
        |> Array.toList
        |> List.map (fun x -> x |> Array.toList)

    )
    |> Array.map (fun block -> solve block 1
        )
    |> Array.sum


part1 () |> printfn "%d"
part2 () |> printfn "%d"
