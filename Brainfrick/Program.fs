open System
open Interpreter


[<EntryPoint>]
let main argv =
    ignore argv

    printfn "%s" Environment.CurrentDirectory

    // if argv.Length = 0 then failwith "wrong length"

    //let filename = @"../../../programs/hanoi.b"
    let filename = @"programs/hanoi.b"

    let i = Interpreter()
    i.InterpretFile filename

    Console.ReadKey true |> ignore


    0 // return an integer exit code
