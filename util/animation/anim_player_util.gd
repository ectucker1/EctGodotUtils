class_name AnimPlayerUtil
extends Object


## Play the given animation if this player is not already playing one of that name.
static func play_if_not(player: AnimationPlayer, name: StringName) -> void:
	if player.current_animation != name:
		player.play(name)
	
## Queue the given animation if an animation with that name is not already in the queue.
static func queue_if_not(player: AnimationPlayer, name: StringName) -> void:
	if player.current_animation != name && !player.get_queue().has(name):
		player.queue(name)

## Starts travel to the given state if the current path does not end there.
static func travel_if_not(state_machine: AnimationNodeStateMachinePlayback, name: StringName) -> void:
	var path = state_machine.get_travel_path()
	if path.is_empty() or path.back() != name:
		state_machine.travel(name);
