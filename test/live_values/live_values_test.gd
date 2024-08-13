extends Node2D

static var TEST_SPEED := 0.5
static var TEST_REVERSE := false

static func _static_init() -> void:
	LiveValues.bind_range_value("TEST_SPEED", func(): return TEST_SPEED, func(val): TEST_SPEED = val, 0.0, 1.0)
	LiveValues.bind_bool_value("TEST_REVERSE", func(): return TEST_REVERSE, func(val): TEST_REVERSE = val)


func _process(delta: float) -> void:
	rotate(-TEST_SPEED if TEST_REVERSE else TEST_SPEED)
	DebugOverlay.add_message(self, "Rotation Speed", str(TEST_SPEED));
	if (TEST_REVERSE):
		DebugOverlay.add_message(self, "Reverse Rotation", "");
