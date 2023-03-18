compile-watch:
	fable watch fsharp -o lib --lang dart
run:
	flutter run --hot --pid-file pidfile
hot-reload:
	cat pidfile |xargs kill -10
hot-reload-watch:
	while inotifywait -e modify lib/*.dart; do cat pidfile |xargs kill -10; done
dev:
	(trap 'kill 0' SIGINT; make compile-watch & prog2 & prog3)