extends Node

## Controls game audio volumes.

@onready var main_volume_range: Range = get_node("MainSlider")
@onready var effects_volume_range: Range = get_node("EffectsSlider")
@onready var music_volume_range: Range = get_node("MusicSlider")

func _ready() -> void:
	init_volumes()
	AudioSettings.volumes_changed.connect(init_volumes)
	main_volume_range.value_changed.connect(update_volumes)
	effects_volume_range.value_changed.connect(update_volumes)
	music_volume_range.value_changed.connect(update_volumes)

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
