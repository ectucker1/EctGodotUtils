class_name AudioStreamCollection
extends Node


static func is_stream(node):
	return node.has_method("play") and "playing" in node and node.has_signal("finished")


var streams := []


var playing: bool:
	get:
		return streams.any(func(stream): return stream.playing)


signal finished


func _ready() -> void:
	for child in get_children():
		if is_stream(child):
			child.finished.connect(func(): finished.emit())
			streams.push_back(child)


func offset_pitch(stream, amount: float) -> void:
	if "pitch_scale" in stream:
		if not stream.has_meta("original_pitch_scale"):
			stream.set_meta("original_pitch_scale", stream.pitch_scale)
		stream.pitch_scale = stream.get_meta("original_pitch_scale") + amount

func offset_volume(stream, amount: float) -> void:
	if "volume_db" in stream:
		if not stream.has_meta("original_volume_db"):
			stream.set_meta("original_volume_db", stream.volume_db)
		stream.volume_db = stream.get_meta("original_volume_db") + amount


func play(from: float = 0.0) -> void:
	pass

func play_if_not():
	if not playing:
		play()

func stop():
	for stream in streams:
		stream.stop()
