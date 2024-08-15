extends Node
## Global pool of sound effects


func play(sound: NodePath) -> void:
	var stream = get_node(sound)
	if AudioStreamCollection.is_stream(stream):
		stream.play()
