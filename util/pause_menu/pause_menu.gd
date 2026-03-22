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


@onready var resume_button: Button = get_node("CanvasLayer/PauseMenu/CenterLayout/Resume")
@onready var exit_button: Button = get_node("CanvasLayer/PauseMenu/CenterLayout/Exit")

@onready var main_volume_range: Range = get_node("CanvasLayer/PauseMenu/VolumeContainer/MainSlider")
@onready var effects_volume_range: Range = get_node("CanvasLayer/PauseMenu/VolumeContainer/EffectsSlider")
@onready var music_volume_range: Range = get_node("CanvasLayer/PauseMenu/VolumeContainer/MusicSlider")

@onready var anim_player: AnimationPlayer = get_node("CanvasLayer/PauseMenu/AnimationPlayer")


func _ready() -> void:
	resume_button.pressed.connect(toggle_shown)
	
	exit_button.pressed.connect(exit)
	if OS.get_name() == "Web":
		exit_button.visible = false
	
	anim_player.animation_finished.connect(anim_finished)
	
	init_volumes()
	AudioSettings.volumes_changed.connect(init_volumes)
	main_volume_range.value_changed.connect(update_volumes)
	effects_volume_range.value_changed.connect(update_volumes)
	music_volume_range.value_changed.connect(update_volumes)

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
		else:
			anim_player.play("Hide")

func anim_finished(name: String) -> void:
	if name == "Hide":
		get_tree().paused = false
		shown = false


func exit() -> void:
	get_tree().quit()


func init_volumes() -> void:
	main_volume_range.value = AudioSettings.main_volume
	effects_volume_range.value = AudioSettings.effects_volume
	music_volume_range.value = AudioSettings.music_volume

func update_volumes(value: float) -> void:
	if main_volume_range.value != AudioSettings.main_volume:
		AudioSettings.main_volume = main_volume_range.value
	if effects_volume_range.value != AudioSettings.effects_volume:
		AudioSettings.effects_volume = effects_volume_range.value
	if music_volume_range.value != AudioSettings.music_volume:
		AudioSettings.music_volume = music_volume_range.value
