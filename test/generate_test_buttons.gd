extends VBoxContainer

const TEST_BUTTON_SCENE := preload("res://test/test_scene_button.gd")


func _ready() -> void:
	var scene_paths: Array[String] = []
	for path in ResourceLoader.list_directory("res://test/"):
		if not path.ends_with("/"):
			continue
		for file in ResourceLoader.list_directory("res://test/" + path):
			if file.ends_with(".tscn") and file.contains("test"):
				scene_paths.append("res://test/" + path + file)
	scene_paths.sort_custom(func(a: String, b: String) -> bool:
		return a.get_file() < b.get_file()
	)
	for scene_path in scene_paths:
		_add_test_button(scene_path)


func _add_test_button(scene_path: String) -> void:
	var button := Button.new()
	button.text = scene_path.get_file().get_basename()
	button.set_script(TEST_BUTTON_SCENE)
	button.target_scene_path = scene_path
	add_child(button)
