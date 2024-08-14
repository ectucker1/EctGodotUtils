extends AudioStreamCollection
## A stream collection that plays it's first member with an increasing pitch.


## The time to wait between plays before resetting the sequence the base pitch.
@export var reset_cooldown := 1.0


## The difference in pitches between each member of the sequence.
@export var pitch_offset = 0.1


var time_since_played := 0.0
var next_play := 0


func play(from: float = 1.0):
	time_since_played = 0.0
	
	var next = streams[0]
	offset_pitch(next, pitch_offset * next_play)
	next.play(from)
	next_play += 1


func _process(delta: float) -> void:
	time_since_played += delta
	if time_since_played >= reset_cooldown:
		next_play = 0
