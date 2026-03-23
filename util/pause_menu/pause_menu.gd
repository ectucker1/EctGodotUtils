extends Node
## A game wide pause menu. Handles showing and hiding using it's own UI action.
## Also handles the toggle fullscreen action.
## Should be disabled on other menus.


## Whether or not to enable the pause menu.
var enabled := true :
	set(value):
		enabled = value
		if shown:
			toggle_shown()

## Whether or not the pause menu is currently shown,
var shown := false


@export var resume_button: Button
@export var main_menu_button: Button
@export var exit_button: Button

@export var anim_player: AnimationPlayer


func _ready() -> void:
	resume_button.pressed.connect(toggle_shown)
	main_menu_button.pressed.connect(main_menu)
	
	exit_button.pressed.connect(exit)
	if OS.get_name() == "Web":
		exit_button.visible = false
	
	anim_player.animation_finished.connect(anim_finished)

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("pause"):
		toggle_shown()
	elif event.is_action_pressed("fullscreen"):
		if DisplayServer.window_get_mode() == DisplayServer.WINDOW_MODE_FULLSCREEN:
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
		else:
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)


func toggle_shown() -> void:
	if not anim_player.is_playing():
		if not shown:
			anim_player.play("Show")
			get_tree().paused = true
			shown = true
			resume_button.grab_focus()
		else:
			anim_player.play("Hide")

func anim_finished(name: String) -> void:
	if name == "Hide":
		get_tree().paused = false
		shown = false

func main_menu() -> void:
	var project_main_scene_path = ProjectSettings.get_setting("application/run/main_scene")
	# Pre-hide menu so we don't mess with puasing over top of the transiton
	anim_player.play("Hide")
	shown = false
	TransitionLayer.transition_to(project_main_scene_path)

func exit() -> void:
	get_tree().quit()
