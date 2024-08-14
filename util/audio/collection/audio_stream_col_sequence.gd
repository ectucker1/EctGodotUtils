extends AudioStreamCollection
## A stream collection that plays its members in sequence.


## The time to wait between plays before resetting the sequence to the first element.
@export var reset_cooldown := 1.0


## Whether or not to repeat the sequence after playing the last element.
@export var repeat = false


var time_since_played := 0.0
var next_play := 0


func play(from: float = 1.0):
	time_since_played = 0.0
	
	var next = streams[next_play]
	next.play(from)
	
	if repeat:
		next_play = wrap(next_play + 1, 0, streams.size())
	else:
		next_play = clamp(next_play + 1, 0, streams.size() - 1)


func _process(delta: float) -> void:
	time_since_played += delta
	if time_since_played >= reset_cooldown:
		next_play = 0
