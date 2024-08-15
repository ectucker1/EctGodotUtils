extends Node
## Add underneath a button node to make it make sounds
## Will play the sounds "Hover" and "Pressed" from GlobalSounds


func _ready() -> void:
	var parent = get_parent()
	if parent is BaseButton:
		parent.mouse_entered.connect(func(): GlobalSounds.play("Hover"))
		parent.pressed.connect(func(): GlobalSounds.play("Pressed"))
