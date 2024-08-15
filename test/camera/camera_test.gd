extends Node2D


func _physics_process(delta: float) -> void:
	if randi() % 30 == 0:
		CameraEffects.add_trauma(0.2)

	if randi() % 60 == 0:
		CameraEffects.add_trauma(0.5)

	if randi() % 20 == 0:
		CameraEffects.hitstop(0.3)

	if randi() % 30 == 0:
		CameraEffects.kickback(Vector2.RIGHT, 64.0)
