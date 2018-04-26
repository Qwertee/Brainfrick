module Interpreter

open System.IO
open System

type Interpreter() =
    let mutable dataPointer = 0
    let mutable instrPointer = 0

    let mutable running = true

    let mutable code: char[] = null

    let mutable memory = Array.zeroCreate 50000: int[] // idk what the max size of this should be, internet says around 30,000

    member this.InterpretFile f =
        code <- File.ReadAllText(f).ToCharArray() // |> Seq.ofArray

        while running do
            this.ProcessInstr(code.[instrPointer])
            instrPointer <- instrPointer + 1
            if instrPointer + 1 > Seq.length code then
                running <- false
    
    member this.ProcessInstr instr =
        let inline charToInt c = int c - int '0'
        match instr with
        // increment data pointer
        | '>' -> dataPointer <- dataPointer + 1

        // decrement data pointer
        | '<' -> dataPointer <- dataPointer - 1

        // increment value at data pointer
        | '+' -> Array.set memory dataPointer (memory.[dataPointer] + 1)

        // decrement value at data pointer
        | '-' -> Array.set memory dataPointer (memory.[dataPointer] - 1)

        // print value at data pointer
        | '.' -> if memory.[dataPointer] = 10 then
                    printf "\n"
                 else
                    printf "%c" (char memory.[dataPointer])

        // read console input into value at data pointer
        | ',' ->
            let key = Console.ReadKey().Key.ToString()
            memory.[dataPointer] <- charToInt(key.Chars(0))

        // if value at data pointer is zero, move instr pointer forward to one after matching ']'
        | '[' -> if 0 = memory.[dataPointer] then
                    let mutable c = 0 // keeps track of the nesting of brackets
                    instrPointer <- instrPointer + 1
                    while (code.[instrPointer]) <> ']' || c <> 0 do
                        let currentItem = code.[instrPointer]
                        if currentItem = '[' then
                            c <- c + 1
                        elif currentItem = ']' && c > 0 then
                            c <- c - 1
                        instrPointer <- instrPointer + 1

        // if value at data pointer is nonzero, move instr pointer back to matching '['
        | ']' -> if 0 <> memory.[dataPointer] then
                    let mutable c = 0
                    instrPointer <- instrPointer - 1
                    while (code.[instrPointer]) <> '[' || c <> 0 do
                        let currentItem = code.[instrPointer]
                        if currentItem = ']' then
                            c <- c + 1
                        elif currentItem = '[' && c > 0 then
                            c <- c - 1                        
                        instrPointer <- instrPointer - 1

        // ignore everything else
        | _   -> () // do nothing on unknown character