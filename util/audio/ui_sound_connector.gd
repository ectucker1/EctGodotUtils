extends Node


func _enter_tree() -> void:
	get_tree().node_added.connect(_node_spawned)

func _node_spawned(node: Node) -> void:
	if node is BaseButton:
		node.mouse_entered.connect(_play_default_hover)
		node.pressed.connect(_play_default_pressed)
	elif node is Slider:
		node.mouse_entered.connect(_play_default_hover)
		node.drag_started.connect(_play_default_pressed)
		node.drag_ended.connect(func(value_changed: bool): _play_default_pressed())

func _play_default_hover():
	GlobalSounds.play("Hover")

func _play_default_pressed():
	GlobalSounds.play("Pressed")
