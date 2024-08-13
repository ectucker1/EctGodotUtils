extends Control

func _ready() -> void:
	var grid = get_node("Grid")
	for value in LiveValues.get_values():
		var label = Label.new()
		label.text = value.name
		grid.add_child(label)
		grid.add_child(value.create_editor())
