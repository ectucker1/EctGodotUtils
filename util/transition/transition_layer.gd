extends Node


@export var anim_player: AnimationPlayer


var transitioning := false
var next := ""


## Play an animation to transition to the given scene.
func transition_to(scene: String) -> void:
	if not transitioning:
		transitioning = true
		next = scene
		
		get_tree().paused = true
		PauseMenu.enabled = false

		anim_player.play("OutIn")

func _switch_scene_to_next():
	if transitioning:
		get_tree().change_scene_to_file(next)

func _finish_transition():
	get_tree().paused = false
	PauseMenu.enabled = true
	transitioning = false
	next = ""
