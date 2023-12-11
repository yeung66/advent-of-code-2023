open System.IO
let input = File.ReadAllLines(@"input/11.txt")

let distance (x1, y1) (x2, y2) = 
    abs (x1 - x2) + abs (y1 - y2)

let part times = 
    let map = input |> Array.map (fun line -> line.ToCharArray())
    let height = map.Length
    let width = map.[0].Length

    let rowsWithoutGalaxies = [0..height - 1] |> List.filter (fun row -> not (map.[row] |> Array.contains '#'))
    let colsWithoutGalaxies = [0..width - 1] |> List.filter (fun col -> not (map |> Array.map (fun row -> row.[col]) |> Array.contains '#'))

    let dotsAfter = 
        map
        |> Array.mapi (fun x row -> 
            row |> Array.mapi (fun y dot -> 
                if dot = '#' then
                    let nx = int64 x +  (int64 (rowsWithoutGalaxies |> List.filter (fun rowIdx -> rowIdx < x) |> List.length)) * (times - 1L)
                    let ny = int64 y + (int64 (colsWithoutGalaxies |> List.filter (fun colIdx -> colIdx < y) |> List.length)) * (times - 1L)

                    Some (nx, ny)
                else
                    None
                

                )
            )
        |> Array.collect id
        |> Array.filter (fun dot -> dot.IsSome)
        |> Array.map (fun dot -> dot.Value) 

    // printfn "%A" dotsAfter

    let distances = 
        [for d1 in dotsAfter do
            for d2 in dotsAfter do
                if d1 <> d2 then
                    yield (d1, d2)]
        |> List.map (fun (d1, d2) -> distance d1 d2)
        |> List.sum
    
    distances / 2L
    
part 2 |> printfn "%d"
part 10 |> printfn "%d"
part 1000000 |> printfn "%d"



    