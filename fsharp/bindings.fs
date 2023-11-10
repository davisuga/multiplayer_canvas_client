namespace Bindings
module Flutter =
    open Fable.Core
    [<ImportMember("dart:ui")>]
    type Pixel(x: int, y: int) =
        class
        end
        

    [<ImportMember("package:flutter_hooks/flutter_hooks.dart")>]
    type ValueNotifier<'t> =
        member _.value: 't = nativeOnly

    [<ImportMember("package:flutter_hooks/flutter_hooks.dart")>]
    let useState<'a> : 'a -> ValueNotifier<'a> = fun (a) -> nativeOnly(a)

    [<ImportMember("dart:async")>]
    type Stream<'t> =
        class
        end


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
    


