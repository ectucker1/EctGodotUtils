extends Node2D
## Node creating a chain of its children simulated by verlet physics.
## This can be used for hair and other dangling animations.


@export var gravity := 40.0

@export var dampening := 0.995


class VerletPoint:
	var old_position: Vector2
	var position: Vector2
	
	var locked: bool
	
	var target: Node2D
	
	func _init(position: Vector2, target: Node2D, locked := false) -> void:
		self.position = position
		self.old_position = position
		self.target = target
		self.locked = locked

class VerletStick:
	var point_1: VerletPoint
	var point_2: VerletPoint
	
	var length: float
	
	func _init(point_1: VerletPoint, point_2: VerletPoint) -> void:
		self.point_1 = point_1
		self.point_2 = point_2
		self.length = (point_1.position - point_2.position).length()


var last_position: Vector2
var points: Array[VerletPoint] = []
var sticks: Array[VerletStick] = []



func _ready() -> void:
	for i in range(0, get_child_count()):
		var child = get_child(i)
		if child is Node2D:
			points.push_back(VerletPoint.new(child.global_position, child, points.is_empty()))
	
	for i in range(0, points.size() - 1):
		sticks.push_back(VerletStick.new(points[i], points[i + 1]))
	
	last_position = position


func _process(delta: float) -> void:
	var offset = last_position - position
	for point in points:
		if point.locked:
			point.position = points[0].target.global_position
			point.old_position = points[0].target.global_position
		else:
			point.position -= offset
			point.old_position -= offset
	
	apply_forces(delta)
	apply_constraints(delta)
	
	for point in points:
		point.target.global_position = point.position
	
	last_position = position

func apply_forces(delta: float) -> void:
	for point in points:
		if not point.locked:
			var vel = (point.position - point.old_position) * dampening
			point.old_position = point.position
			point.position += vel
			point.position += Vector2.DOWN * gravity * delta * delta

func apply_constraints(delta: float) -> void:
	for i in range(0, 50):
		for stick in sticks:
			var stick_center = (stick.point_1.position + stick.point_2.position) * 0.5
			var stick_dir = (stick.point_1.position - stick.point_2.position).normalized()
			if not stick.point_1.locked:
				stick.point_1.position = stick_center + stick_dir * stick.length * 0.5
			if not stick.point_2.locked:
				stick.point_2.position = stick_center - stick_dir * stick.length * 0.5
