class_name LVRange
extends RefCounted


const RANGE_ITEM_SCENE := preload("res://util/live_values/range_item.tscn")


var model
var name: String
var getter: Callable
var setter: Callable
var min: float
var max: float

func _init(model, name: String, getter: Callable, setter: Callable, min: float, max: float) -> void:
	self.model = model
	self.name = name
	self.getter = getter
	self.setter = setter
	self.min = min
	self.max = max


func get_val() -> float:
	return getter.call()

func set_val(val: float) -> void:
	setter.call(val)
	model.modified = true

func create_editor() -> Control:
	var editor = RANGE_ITEM_SCENE.instantiate()
	editor.bind(model, self)
	return editor
