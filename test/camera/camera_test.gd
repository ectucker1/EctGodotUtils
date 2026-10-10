extends Control


func _ready() -> void:
	$Shake.pressed.connect(func(): CameraEffects.add_trauma(0.2))
	$BigShake.pressed.connect(func(): CameraEffects.add_trauma(0.6))
	$Hitstop.pressed.connect(func(): CameraEffects.hitstop(0.3))
	$Kickback.pressed.connect(func(): CameraEffects.kickback(Vector2.RIGHT, 32.0))
