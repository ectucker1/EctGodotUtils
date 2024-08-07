extends Node


@onready
var text_display = DebugOverlayDisplay.find_child("DebugTextDisplay")

@onready
var in_world_display = InWorldOverlay


func _input(event: InputEvent) -> void:
	if (event.is_action_pressed("debug_show")):
		DebugOverlayDisplay.visible = !DebugOverlayDisplay.visible;


## Adds a text message to the debug overlay.
## The message will display in the format "[param prefix]: [param content]".
## The message will display for [param time] seconds.
## Messages with lower [param priority] are displayed earlier.
## If an existing message already has the same [param src], [param prefix], and [param priority],
## that message will be replaced.
func add_message(src: Object, prefix: String, content: String, color: Color = Color.WHITE, priority: int = 10000, time: float = 0.1):
	text_display.add_message(src, prefix, content, color, priority, time)


## Draws a line from [param from] in global coordinates to [param vector] away in global coordinates.
func draw_vector_from(from: Vector2, vector: Vector2, color: Color) -> void:
	in_world_display.draw_vector_from(from, vector, color)


## Draws a line from [param from] in global coordinates to [param to] in global coordinates.
func draw_line_between(from: Vector2, to: Vector2, color: Color) -> void:
	in_world_display.draw_line_between(from, to, color)


## Draws a circle at [param point].
func draw_point(point: Vector2, color: Color) -> void:
	in_world_display.draw_point(point, color)
	
