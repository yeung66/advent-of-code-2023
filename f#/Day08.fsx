open System.IO

let readInput = 
    File.ReadAllText("input/08.txt")

type Navigation =
    {
        cur: string;
        left: string;
        right: string;
    }

let parseInput (input: string) = 
    let parts = input.Split("\n\n")
    let instructions = parts.[0].ToCharArray()
    let navigations = 
        parts.[1].Split("\n") |> Array.map (fun line -> 
        let pos = line.Substring(0,3)
        let left = line.Substring(7,3)
        let right = line.Substring(12,3)

        (pos, { cur = pos; left = left; right = right }))
        |> Map.ofArray

    (instructions, navigations)

let nav direction navigation = 
    match direction with
    | 'L' -> navigation.left
    | 'R' -> navigation.right
    | _ -> navigation.cur

let part1 input = 
    let (instructions, navigations) = parseInput input

    let mutable pos = "AAA"
    let endPos = "ZZZ"
    let mutable cnt = 0

    while pos <> endPos do
        let direction = instructions.[cnt % instructions.Length]
        pos <- nav direction navigations.[pos]
        cnt <- cnt + 1
    
    cnt

let part2 input = 
    let (instructions, navigations) = parseInput input

    let mutable positions = navigations.Keys |> Seq.cast<string> |> Seq.toArray |> Array.filter (fun pos -> pos.EndsWith("A"))

    let stepsToEnd = 
            positions
            |> Array.map (fun start -> 
                let mutable pos = start
                let mutable int = 0
                let mutable cnt = int64 0
                while not (pos.EndsWith("Z")) do
                    let direction = instructions.[int % instructions.Length]
                    pos <- nav direction navigations.[pos]
                    cnt <- cnt + (int64 1)
                    int <- (int + 1) % instructions.Length
            
                cnt
            ) 

    let gcd a b =
        let rec gcd' a b =
            if b = 0L then a else gcd' b (a % b)
        gcd' (abs a) (abs b)

    // printfn "%A" stepsToEnd
        
    stepsToEnd |> Array.reduce (fun acc x -> x * acc / (gcd x acc))
    

    
        
   
// 4592948212823752704
// 9177460370549


readInput
|> part1
|> printfn "Part 1: %d"

readInput
|> part2
|> printfn "Part 2: %d"

    
        