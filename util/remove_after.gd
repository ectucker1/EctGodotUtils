extends Node


## The amount of time to wait before removing this node.
@export
var lifetime := 1.0


var time := 0.0


func _process(delta: float) -> void:
	time += delta
	if time >= lifetime:
		queue_free()
