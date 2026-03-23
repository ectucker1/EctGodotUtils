extends Node
## Global pool of sound effects


# When true, triggers of new sounds are disabled
var pause_triggers: bool = false


func play(sound: NodePath) -> void:
	if pause_triggers:
		return
	
	var stream = get_node(sound)
	if AudioStreamCollection.is_stream(stream):
		stream.play()
