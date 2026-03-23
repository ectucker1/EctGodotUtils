extends Node2D

@export
var test_speed := 0.5
@export
var test_reverse := false

func _process(delta: float) -> void:
	rotate(-test_speed if test_reverse else test_speed)
	DebugOverlay.add_message(self, "Rotation Speed", str(test_speed));
	if (test_reverse):
		DebugOverlay.add_message(self, "Reverse Rotation", "");
