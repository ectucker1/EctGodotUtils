extends Node


const FILE_PATH := "user://settings.ini"
const SECTION := "audio"

const MAIN_VOLUME_SETTING := "main_volume"
const EFFECTS_VOLUME_SETTING := "effects_volume"
const MUSIC_VOLUME_SETTING := "music_volume"


var last_updated := INF
var unsaved := false


signal volumes_changed


var main_volume := 50.0 :
	set(value):
		main_volume = clamp(value, 0.0, 100.0)
		AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Master"), linear_to_db(main_volume / 100.0))
		mark_changed()

var effects_volume := 50.0 :
	set(value):
		effects_volume = clamp(value, 0.0, 100.0)
		AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Effects"), linear_to_db(effects_volume / 100.0))
		mark_changed()

var music_volume := 50.0 :
	set(value):
		music_volume = clamp(value, 0.0, 100.0)
		AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Music"), linear_to_db(music_volume / 100.0))
		mark_changed()


func mark_changed() -> void:
	last_updated = 0.0
	unsaved = true
	volumes_changed.emit()


func _ready() -> void:
	load_config()

func _physics_process(delta: float) -> void:
	if unsaved:
		last_updated += delta
		if last_updated >= 1.0:
			save_config()


func save_config() -> void:
	if OS.get_name() != "Web":
		var config = ConfigFile.new()
		config.set_value(SECTION, MAIN_VOLUME_SETTING, main_volume)
		config.set_value(SECTION, EFFECTS_VOLUME_SETTING, effects_volume)
		config.set_value(SECTION, MUSIC_VOLUME_SETTING, music_volume)
		config.save(FILE_PATH)
	unsaved = false

func load_config() -> void:
	if OS.get_name() != "Web":
		var config = ConfigFile.new()
		var error = config.load(FILE_PATH)
		if error == OK:
			main_volume = config.get_value(SECTION, MAIN_VOLUME_SETTING, 50.0)
			effects_volume = config.get_value(SECTION, EFFECTS_VOLUME_SETTING, 100.0)
			music_volume = config.get_value(SECTION, MUSIC_VOLUME_SETTING, 100.0)
