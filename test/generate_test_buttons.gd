extends VBoxContainer

const TEST_BUTTON_SCENE := preload("res://test/test_scene_button.gd")


func _ready() -> void:
	var test_dir := DirAccess.open("res://test/")
	var scene_paths: Array[String] = []
	_scan_for_test_scenes(test_dir, "res://test/", true, scene_paths)
	scene_paths.sort_custom(func(a: String, b: String) -> bool:
		return a.get_file() < b.get_file()
	)
	for scene_path in scene_paths:
		_add_test_button(scene_path)


func _scan_for_test_scenes(dir: DirAccess, path: String, is_root: bool, results: Array[String]) -> void:
	dir.list_dir_begin()
	var file_name := dir.get_next()
	while file_name != "":
		var full_path := path + file_name
		if dir.current_is_dir():
			var sub_dir := DirAccess.open(full_path)
			if sub_dir:
				_scan_for_test_scenes(sub_dir, full_path + "/", false, results)
		elif file_name.ends_with(".tscn") and file_name.contains("test") and not is_root:
			results.append(full_path)
		file_name = dir.get_next()
	dir.list_dir_end()


func _add_test_button(scene_path: String) -> void:
	var button := Button.new()
	button.text = scene_path.get_file().get_basename()
	button.set_script(TEST_BUTTON_SCENE)
	button.target_scene_path = scene_path
	add_child(button)
