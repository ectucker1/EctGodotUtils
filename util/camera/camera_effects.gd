extends Node
## Provides common camera effects, such as screenshake and hitstop.


## Screenshake magnitude at maximum trauma, in pixels 
const MAX_SCRENSHAKE_MAGNITUDE := 128.0

## Trauma decay rate, in trauma/s
## This correlates quadratically with the actual screenshake offset
const TRAUMA_DECAY_RATE := 1.0

## Kickback decay rate, in pixels/s
const KICKBACK_DECAY_RATE := 64.0


var screenshake_offset := Vector2.ZERO

var kickback_dir := Vector2.ZERO
var kickback_length := 0.0

var offset := Vector2.ZERO

var trauma := 0.0 :
	set(value):
		trauma = clamp(value, 0.0, 1.0)

var time := 0.0

var hitstop_end_time := 0.0

var noise: FastNoiseLite


## Adds an amount of trauma for screenshake.
func add_trauma(amount: float) -> void:
	trauma = clamp(trauma + amount, 0.0, 1.0)


## Kickback the camera in a direction, will fade over time.
## Strength is in pixels.
func kickback(dir: Vector2, strength: float) -> void:
	kickback_dir = dir.normalized()
	kickback_length = strength


## Freeze the game for a duration in seconds.
## This works by changing Engine.TimeScale.
func hitstop(duration: float) -> void:
	Engine.time_scale = 0.0
	hitstop_end_time = Time.get_ticks_msec() + duration * 1000


## Returns the camera offset which should be applied to cameras using these effects.
func get_offset() -> Vector2:
	return offset


func _ready() -> void:
	noise = FastNoiseLite.new()
	noise.fractal_octaves = 4

func _process(delta: float) -> void:
	time += delta
	
	trauma = clamp(trauma - delta * TRAUMA_DECAY_RATE, 0.0, 1.0)
	screenshake_offset.x = MAX_SCRENSHAKE_MAGNITUDE * trauma * trauma * noise.get_noise_1d(time)
	screenshake_offset.y = MAX_SCRENSHAKE_MAGNITUDE * trauma * trauma * noise.get_noise_1d(-time)
	
	kickback_length = clamp(kickback_length - delta * KICKBACK_DECAY_RATE, 0, INF)
	
	offset = screenshake_offset + kickback_dir * kickback_length
	
	if Time.get_ticks_msec() > hitstop_end_time:
		Engine.time_scale = 1.0
