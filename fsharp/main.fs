module Main

open Flutter.Foundation
open Flutter.Widgets
open Flutter.Material
open Flutter.Services
open Bindings.Flutter
open Elmish
open Elmish.Flutter
open System.Collections.Generic

let dictOfList (list: ('a * 'b) list) =
    let dict = new Dictionary<'a, 'b>()
    list |> List.iter (fun (k, v) -> dict.Add(k, v))
    dict

open Fable.Core

module CanvasScreen =
    open Dart
    type Pixel = { color: Color }
    type Model = { id: string; value: Pixel list list }

type CreateBoardScreen(?key: Key) =
    inherit StatelessWidget(?key = key)

    let query = "mutation createCanvas { createCanvas(rows: 1024, columns: 1024) { id }}"
    
    let onClickCreate () =
        let channel = WebSocketChannel.connect (Uri.parse ("ws://localhost:8080"))
        let x = channel.sink.add ("Hello")
        x

    override _.build(context) =
        MaterialApp(
            title = "Multiplayer Canvas",
            home =
                (Scaffold(
                    appBar = AppBar(title = Text("Multiplayer Canvas")),
                    body =
                        Center(
                            child = TextButton(child = Text("Create new canvas"), onPressed = fun () -> onClickCreate() |> ignore)
                        )
                )
                :> Widget)
        )


type MyApp(?key: Key) =

    inherit StatelessWidget(?key = key)

    let routes =
        dictOfList (
            [ ("/", (fun context -> CreateBoardScreen(?key = key).build (context)))
              ("/board", (fun context -> CreateBoardScreen(?key = key).build (context))) ]
        )

    override _.build(context) =
        let x = useState(None)
        MaterialApp(
            routes = routes,
            title = "Multiplayer Canvas"

        )

let main () = MyApp() |> runApp
