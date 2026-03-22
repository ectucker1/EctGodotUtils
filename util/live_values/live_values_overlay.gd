extends Control


@onready var grid = get_node("Grid")


func _ready() -> void:
	for value in LiveValues.get_values():
		add_value(value)
	LiveValues.value_bound.connect(add_value)

func add_value(value) -> void:
	var label = Label.new()
	label.text = value.name
	grid.add_child(label)
	grid.add_child(value.create_editor())
