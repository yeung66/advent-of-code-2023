open System

let readInput =
    System.IO.File.ReadAllText(@"input/01.txt")

let solve (input: String) =
    input.Split("\n")
    |> Array.map (fun line ->
        
        let first = line.ToCharArray() |> Seq.find (fun c -> Char.IsDigit c)
        let last = line.ToCharArray() |> Seq.findBack (fun c -> Char.IsDigit c)
        int $"{first}{last}"
        )
    |> Array.sum

// {"one": 1, "two": 2, ...}
let numsMapping = 
    let letters = 
        [|"one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine" |]
        |> Array.mapi (fun i s -> (s, i + 1))

    let digits = 
        [|"1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
        |> Array.mapi (fun i s -> (s, i + 1))

    Array.concat [letters; digits]

let solve2 (input: String) = 
    input.Split("\n")
    |> Array.map (fun line ->
            let (first, _) = 
                numsMapping 
                |> Array.map(fun (s, i) -> (s, line.IndexOf(s))) 
                |> Array.filter (fun (s, i) -> i >= 0)
                |> Array.minBy (fun (s, i) -> i)

            let (last, _) = 
                numsMapping 
                |> Array.map(fun (s, i) -> (s, line.LastIndexOf(s))) 
                |> Array.filter (fun (s, i) -> i >= 0)
                |> Array.maxBy (fun (s, i) -> i)

            let findMapping (s: String) = 
                numsMapping 
                |> Array.find (fun (s1, i) -> s1 = s)
                |> snd

            int $"{findMapping first}{findMapping last}"
        )
    |> Array.sum


readInput
|> solve2
|> printfn "%d"