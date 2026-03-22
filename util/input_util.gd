class_name InputUtil
extends Node


## Gets the mouse position in global coordinates.
static func get_global_mouse_position(node: Node) -> Vector2:
	var viewport = node.get_viewport()
	var viewportPos = viewport.get_mouse_position()
	return viewport.canvas_transform.affine_inverse() * viewportPos
