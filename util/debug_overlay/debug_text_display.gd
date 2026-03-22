extends RichTextLabel


var messages: Array


class DebugMessage:
	var src_id: int
	var prefix: String
	var content: String
	var color: Color
	var priority: int
	var time: float
	
	func _init(src_id: int, prefix: String, content: String, color: Color, priority: int, time: float):
		self.src_id = src_id
		self.prefix = prefix
		self.content = content
		self.color = color
		self.priority = priority
		self.time = time


func add_message(src: Object, prefix: String, content: String, color: Color, priority: int = 10000, time: float = 0.1):
	var src_id := src.get_instance_id()
	var message := DebugMessage.new(src_id, prefix, content, color, priority, time)
	messages = messages.filter(func(message): return message.src_id != src_id or message.prefix != prefix or message.priority != priority)
	messages.push_back(message)


func _process(delta: float) -> void:
	add_message(self, "FPS", str(Engine.get_frames_per_second()), Color.WHITE)
	
	clear()
	messages.sort_custom(func(a, b): b.priority < a.priority or b.src_id < a.src_id or a.prefix.naturalnocasecmp_to(b.prefix) < 0)
	for message in messages:
		push_color(message.color)
		add_text("%s: %s\n" % [message.prefix, message.content])
		message.time -= delta
	
	messages = messages.filter(func(message): return message.time > 0)
