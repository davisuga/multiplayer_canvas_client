module Main

open Flutter.Foundation
open Flutter.Widgets
open Flutter.Material
open Flutter.Services

open Elmish
open Elmish.Flutter

open Fable.Core
[<ImportMember("dart:ui")>]
type Pixel(x:int, y:int) =
    class end

[<ImportMember("dart:async")>]
type Stream<'t> =
    class end


[<ImportMember("dart:convert")>]
let jsonDecode<'T> : string -> 'T = nativeOnly


[<Global>]
type Uri =
    static member parse: string -> Uri = nativeOnly


[<Global>]
type Sink<'t> = 
    member _.add: 't -> unit = nativeOnly 


[<ImportMember("package:web_socket_channel/web_socket_channel.dart")>]
type WebSocketChannel =
    static member connect: Uri -> WebSocketChannel = nativeOnly
    member _.stream: Stream<string> = nativeOnly
    member _.sink: Sink<string> = nativeOnly

[<ImportDefault("dart:core")>]
module CanvasScreen= 
    open Dart

    type Model = {offset: Offset; color: Color}
// Simple Todo app based on https://www.section.io/engineering-education/how-to-build-a-flutter-todo-app/

module App =

    type Todo = { id: int; title: string }
    // let x = new Sink<string>()
    type Model = { todos: Todo list }
    let channel = WebSocketChannel.connect(Uri.parse("ws://localhost:8080"))
    let x = channel.sink.add("Hello")


    type Msg =
        | AddTodo of title: string
        | DeleteTodo of id: int
        | EditTodo of id: int * text: string

    let init () : Model * Cmd<Msg> = { todos = [] }, Cmd.none

    let update msg model : Model * Cmd<Msg> =
    
        match msg with
        | AddTodo title ->
            let id = (List.length model.todos) + 1

            { model with
                todos = { id = id; title = title } :: model.todos
            },
            Cmd.none

        | DeleteTodo id ->
            let todos = model.todos |> List.filter (fun t -> t.id <> id)
            { model with todos = todos }, Cmd.none

        | EditTodo (id, title) ->
            let todos =
                model.todos
                |> List.map (fun t -> if t.id = id then { t with title = title } else t)

            { model with todos = todos }, Cmd.none

    let displayDialog text dispatch (context: BuildContext) =
        showDialog (
            context = context,
            builder =
                (fun context ->
                    AlertDialog(
                        title = Text("Add some tasks to your list"),
                        content =
                            TextFormField(
                                initialValue = text,
                                autofocus = true,
                                textInputAction = TextInputAction.``done``,
                                onFieldSubmitted =
                                    (fun text ->
                                        Navigator.``of``(context).pop ()
                                        text |> dispatch),
                                decoration = InputDecoration(hintText = "Enter task here")
                            ),
                        actions =
                            [|
                                MaterialButton(
                                    child = Text("CANCEL"),
                                    onPressed = fun () -> Navigator.``of``(context).pop ()
                                )
                            |]
                    ))
        )

    let buildTodos (model: Model) dispatch context =
        let todos =
            model.todos
            |> List.map (fun t ->
                ListTile(
                    title =
                        Row
                            [|
                                Expanded(Text(t.id.ToString()))
                                Expanded(Text(t.title))

                                IconButton(
                                    icon = Icon(Icons.edit),
                                    onPressed =
                                        fun () ->
                                            let dispatch text = EditTodo(t.id, text) |> dispatch
                                            displayDialog t.title dispatch context |> ignore
                                )
                                IconButton(icon = Icon(Icons.delete), onPressed = fun () -> DeleteTodo t.id |> dispatch)
                            |]
                )
                :> Widget)
            |> List.toArray

        ListView(children = todos)

    let view (model: Model) (dispatch: Msg -> unit) context : Widget =
        Scaffold(
            appBar = AppBar(title = Text("Multiplayer Canvas")),
            body =
                Row
                    [|
                        Expanded(flex = 1, child = SizedBox.shrink ())
                        Expanded(flex = 2, child = buildTodos model dispatch context)
                        Expanded(flex = 1, child = SizedBox.shrink ())
                    |],
            floatingActionButton =
                FloatingActionButton(
                    child = Icon(Icons.task),
                    tooltip = "Add Shit",
                    onPressed = fun () -> displayDialog "" (AddTodo >> dispatch) context |> ignore
                )
        )

open App
type CreateBoardScreen(?key: Key) =
    inherit StatelessWidget(?key = key)

    override _.build(context) =
        MaterialApp(title = "Multiplayer Canvas", home = ElmishWidget.From(init, update, view))    

type MyApp(?key: Key) =
    inherit StatelessWidget(?key = key)
    let routes = dict ["/",fun context -> CreateBoardScreen().build(context)]

    override _.build(context) =
        MaterialApp(
        routes=routes,
        title = "Multiplayer Canvas"
        
)

type CreateBoardScreen(?key: Key) =
    inherit StatelessWidget(?key = key)

    override _.build(context) =
        MaterialApp(title = "Multiplayer Canvas", home = ElmishWidget.From(init, update, view))    

let main () = MyApp() |> runApp