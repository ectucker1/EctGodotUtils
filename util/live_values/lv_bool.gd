class_name LVBool
extends RefCounted


const SWITCH_ITEM_SCENE := preload("res://util/live_values/switch_item.tscn")


var model
var name: String
var getter: Callable
var setter: Callable

func _init(model, name: String, getter: Callable, setter: Callable) -> void:
	self.model = model
	self.name = name
	self.getter = getter
	self.setter = setter


func get_val() -> bool:
	return getter.call()

func set_val(val: bool) -> void:
	setter.call(val)
	model.modified = true

func create_editor() -> Control:
	var editor = SWITCH_ITEM_SCENE.instantiate()
	editor.bind(model, self)
	return editor
