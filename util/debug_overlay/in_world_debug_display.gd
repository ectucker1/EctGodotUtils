extends Node2D


var drawables := []


class DebugVector:
	var from: Vector2
	var vector: Vector2
	var color: Color

	func _init(from: Vector2, vector: Vector2, color: Color) -> void:
		self.from = from
		self.vector = vector
		self.color = color
	
	func draw(canvas: CanvasItem) -> void:
		canvas.draw_line(from, from + vector, color, 5)


class DebugPoint:
	var point: Vector2
	var color: Color
	
	func _init(point: Vector2, color: Color) -> void:
		self.point = point
		self.color = color
	
	func draw(canvas: CanvasItem) -> void:
		canvas.draw_circle(point, 8, color)


func draw_vector_from(from: Vector2, vector: Vector2, color: Color) -> void:
	drawables.push_back(DebugVector.new(from, vector, color))
	queue_redraw()


func draw_line_between(from: Vector2, to: Vector2, color: Color) -> void:
	drawables.push_back(DebugVector.new(from, to - from, color))
	queue_redraw()


func draw_point(point: Vector2, color: Color) -> void:
	drawables.push_back(DebugPoint.new(point, color))
	queue_redraw()


func _draw() -> void:
	for drawable in drawables:
		drawable.draw(self)
	drawables.clear()


func _process(delta: float) -> void:
	visible = DebugOverlayDisplay.visible
