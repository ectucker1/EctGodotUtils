class_name EffectsCamera2D
extends Camera2D
## Represents a camera which will have the offset from CameraEffects applied every frame.
## Should be extended for more advanced camera behavior.


func _process(_delta: float) -> void:
	offset = CameraEffects.get_offset()
