extends Sprite2D


@onready var anim_player: AnimationPlayer = get_node("AnimationPlayer")

var canceled := false


func _ready() -> void:
	var stop: Button = get_parent().get_node("Stop")
	stop.pressed.connect(stop_pressed)
	
	test_routine()


func test_routine() -> void:
	DebugOverlay.add_message(self, "Routine", "Started")
	
	await get_tree().create_timer(2.0).timeout
	if canceled:
		return
	
	for i in range(0, 100):
		await get_tree().physics_frame
		if canceled:
			return
		global_position += Vector2.RIGHT * get_physics_process_delta_time() * 50.0

	anim_player.play("Test")
	await anim_player.animation_finished
	
	for i in range(0, 100):
		await get_tree().physics_frame
		if canceled:
			return
		global_position += Vector2.LEFT * get_physics_process_delta_time() * 50.0
	
	DebugOverlay.add_message(self, "Routine", "Finished")


func stop_pressed() -> void:
	canceled = true
