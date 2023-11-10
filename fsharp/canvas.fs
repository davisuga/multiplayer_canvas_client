
module Canvas
open Flutter.Widgets
open Flutter.Foundation

open Dart
open Elmish
type Pixel = { color: Color }
type Model = { id: string; value: Pixel list list }

type Msg =
    | SetPixel of Pixel * int * int
    | SetCanvas of Model

let init () = { id = ""; value = [] }, Cmd.none

let update msg model =
    match msg with
    | SetPixel (pixel, x, y) ->
        let mutable newVal = model.value  |> List.toArray
        newVal.[x].[y] <- pixel
        
        { model with value = newVal }, Cmd.none
    | SetCanvas canvas -> canvas, Cmd.none

    
type Screen (?key: Key) =
    inherit StatelessWidget(?key = key)
    
    override _.build(ctx: BuildContext): Widget = 
        failwith "Not Implemented"

