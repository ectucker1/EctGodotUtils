class_name LiveValuesModel
extends Node


const DATA_PATH := "res://livevalues.ini"
const MIDI_PATH := "res://livevalues_midi.ini"
const CONFIG_SECTION := "LiveValues"


# List of all bound values
var value_bindings := []
# Maps from values to MIDI controller info
var value_midi := {}


var modified := false
var next_save := 0.0


func bind_range_value(name: String, getter: Callable, setter: Callable, min: float, max: float):
	value_bindings.push_back(LVRange.new(self, name, getter, setter, min, max))

func bind_bool_value(name: String, getter: Callable, setter: Callable):
	value_bindings.push_back(LVBool.new(self, name, getter, setter))


func get_values() -> Array:
	return value_bindings

func get_value(name: String):
	for val in value_bindings:
		if val.name == name:
			return val
	return null 


func get_midi(val: LVRange) -> MIDIValueChannel:
	return value_midi.get(val.name)

func put_midi(val: LVRange, channel: MIDIValueChannel) -> void:
	value_midi[val.name] = channel

func erase_midi(val: LVRange) -> void:
	value_midi.erase(val.name)


func load_config() -> void:
	if FileAccess.file_exists(DATA_PATH):
		var config = ConfigFile.new()
		if config.load(DATA_PATH) == OK:
			for name in config.get_section_keys(CONFIG_SECTION):
				var val = get_value(name)
				if val:
					val.set_val(config.get_value(CONFIG_SECTION, name, val.get_val()))
	
	if FileAccess.file_exists(MIDI_PATH):
		var midi_config = ConfigFile.new()
		if midi_config.load(MIDI_PATH) == OK:
			for name in midi_config.get_sections():
				var channel = midi_config.get_value(name, "channel")
				var controller = midi_config.get_value(name, "cc")
				var min = midi_config.get_value(name, "min")
				var max = midi_config.get_value(name, "max")
				var val = get_value(name)
				if val is LVRange:
					put_midi(val, MIDIValueChannel.new(channel, controller, min, max))

func save_config() -> void:
	var config = ConfigFile.new()
	for val in value_bindings:
		config.set_value(CONFIG_SECTION, val.name, val.get_val())
	config.save(DATA_PATH)
	
	var midi_config = ConfigFile.new()
	for val in value_midi:
		var channel = value_midi.get(val)
		midi_config.set_value(val, "channel", channel.channel)
		midi_config.set_value(val, "cc", channel.controller)
		midi_config.set_value(val, "min", channel.min)
		midi_config.set_value(val, "max", channel.max)
	midi_config.save(MIDI_PATH)


func _ready() -> void:
	load_config()

func _process(delta: float) -> void:
	if modified and next_save <= 0:
		next_save = 0.5
	if next_save > 0:
		next_save -= delta
		if  next_save <= 0:
			save_config()
