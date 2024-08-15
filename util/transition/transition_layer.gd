extends Node


@onready var anim_player: AnimationPlayer = get_node("CanvasLayer/Transition/AnimationPlayer")


var transitioning := false
var next := ""


## Play an animation to transition to the given scene.
func transition_to(scene: String) -> void:
	if not transitioning:
		transitioning = true
		next = scene
		
		get_tree().paused = true
		PauseMenu.enabled = false
		
		anim_player.play("Out")
		await anim_player.animation_finished
		
		get_tree().change_scene_to_file(next)
		
		anim_player.play("In")
		await anim_player.animation_finished
		
		get_tree().paused = false
		PauseMenu.enabled = true
