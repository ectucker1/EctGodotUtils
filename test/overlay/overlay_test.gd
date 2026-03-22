extends Node


@onready
var sprite_1 := $Sprite1
@onready
var sprite_2 := $Sprite2


func _process(delta: float) -> void:
	DebugOverlay.add_message(self, "Sprite2D 1 Position", str(sprite_1.global_position), Color.AQUA)
	if randi() % 120 == 0:
		DebugOverlay.add_message(self, "Event", "Something Happened", Color.FUCHSIA, 1, 0.5)
	
	DebugOverlay.draw_line_between(sprite_1.global_position, sprite_2.global_position, Color.RED)
	DebugOverlay.draw_vector_from(sprite_1.global_position, Vector2.UP * 50.0, Color.ORANGE)
	DebugOverlay.draw_point(sprite_2.global_position, Color.FOREST_GREEN)
