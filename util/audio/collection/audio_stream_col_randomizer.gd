extends AudioStreamCollection
## A stream collection which plays a random member each time.


## The maximum difference of a played pitch scale from the original.
@export var pitch_offset_range := 0.0

## The maximum difference of a played volume from the original.
@export var volume_offset_range := 0.0


func play(from: float = 0.0):
	stop()
	
	var pick: Node = streams.pick_random()
	offset_pitch(pick, randf() * pitch_offset_range)
	offset_volume(pick, randf() * volume_offset_range)
	pick.play(from)
