class_name MIDIValueChannel
extends RefCounted


var channel: int
var controller: int
var min: float
var max: float


func _init(channel: int, controller: int, min: float, max: float) -> void:
	self.channel = channel
	self.controller = controller
	self.min = min
	self.max = max


func copy_range(new_min: float, new_max: float) -> MIDIValueChannel:
	return MIDIValueChannel.new(channel, controller, new_min, new_max)


func matches(event: InputEventMIDI) -> bool:
	return event.message == MIDI_MESSAGE_CONTROL_CHANGE \
		and event.channel == channel \
		and event.controller_number == controller


func scale_val(event: InputEventMIDI) -> float:
	return min + (max - min) * event.controller_value / 127.0


func _to_string() -> String:
	return  "CHNL %d CC %d" % [channel, controller]
