extends Button

@export
var target_scene_path: String

func _pressed() -> void:
	TransitionLayer.transition_to(target_scene_path)
