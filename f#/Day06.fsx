let inputStr =
    """
46     80     78     66
214   1177   1402   1024
    """
        .Trim()

let input2 = 
    let nums = 
        inputStr.Split("\n")
        |> Array.map (fun line ->
            line.Split(" ")
            |> String.concat ""
            |> uint64)

    [|(nums.[0], nums.[1])|]

let input1 =
    let nums =
        inputStr.Split("\n")
        |> Array.map (fun line ->
            line.Split(" ")
            |> Array.filter (fun num -> num <> "")
            |> Array.map (fun num -> uint64 num))



    nums.[0] |> Array.mapi (fun i num -> (num, nums.[1].[i]))

let part (input: (uint64 * uint64) array) =
    input
    |> Array.map (fun (seconds, record) ->
        seq { 0..(int seconds) }
        |> Seq.map (fun i -> (seconds - uint64 i) * uint64 i)
        |> Seq.filter (fun dis -> dis > record)
        |> Seq.length)
    |> Array.fold (*) 1




part input1 |> printfn "%d"
part input2 |> printfn "%d"

