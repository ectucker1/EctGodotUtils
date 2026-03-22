extends Control


const SLIDER_STEP := 0.000001


var model: LiveValuesModel
var val: LVRange

var midi_learning := false
var midi_channel: MIDIValueChannel = null


@onready var slider: HSlider = get_node("SliderNumbox/Slider")
@onready var numbox: SpinBox = get_node("SliderNumbox/Numbox")

@onready var midi_learn: Button = get_node("MIDI/LearnButton")
@onready var midi_min: SpinBox = get_node("MIDI/MidiZero")
@onready var midi_max: SpinBox = get_node("MIDI/MidiMax")


func bind(model: LiveValuesModel, val: LVRange) -> void:
	self.model = model
	self.val = val

func _ready() -> void:
	slider.min_value = val.min
	slider.max_value = val.max
	slider.step = SLIDER_STEP
	slider.value = val.get_val()
	slider.value_changed.connect(slider_changed)
	
	numbox.min_value = val.min
	numbox.max_value = val.max
	numbox.step = SLIDER_STEP
	numbox.value = val.get_val()
	numbox.value_changed.connect(numbox_changed)
	
	midi_min.min_value = val.min
	midi_min.max_value = val.max
	midi_min.step = SLIDER_STEP
	midi_min.value = val.min
	
	midi_max.min_value = val.min
	midi_max.max_value = val.max
	midi_max.step = SLIDER_STEP
	midi_max.value = val.max
	
	midi_channel = model.get_midi(val)
	if midi_channel:
		open_os_midi()
		midi_min.value = midi_channel.min
		midi_max.value = midi_channel.max
		midi_learning = false
		midi_learn.text = str(midi_channel)
	
	midi_min.value_changed.connect(midi_range_changed)
	midi_max.value_changed.connect(midi_range_changed)
	midi_learn.pressed.connect(midi_learn_pressed)


func update_val(value: float) -> void:
	val.set_val(value)


func slider_changed(value: float) -> void:
	numbox.value = value
	update_val(value)

func numbox_changed(value: float) -> void:
	slider.value = value
	update_val(value)


func open_os_midi():
	if OS.get_connected_midi_inputs().is_empty():
		OS.open_midi_inputs()

func midi_range_changed(value: float) -> void:
	if midi_channel:
		midi_channel = midi_channel.copy_range(midi_min.value, midi_max.value)
		model.put_midi(val, midi_channel)

func midi_learn_pressed() -> void:
	if not midi_learning:
		if midi_channel:
			midi_learn.text = "LEARN"
			midi_channel = null
			model.erase_midi(val)
			midi_learning = false
		else:
			open_os_midi()
			midi_learn.text = "..."
			midi_learning = true
	else:
		midi_learn.text = "LEARN"
		midi_channel = null
		model.erase_midi(val)
		midi_learning = false

func _input(event: InputEvent) -> void:
	if event is InputEventMIDI:
		if midi_learning and event.message == MIDI_MESSAGE_CONTROL_CHANGE:
			midi_channel = MIDIValueChannel.new(
				event.channel,
				event.controller_number,
				midi_min.value, 
				midi_max.value
			)
			model.put_midi(val, midi_channel)
			midi_learning = false
			midi_learn.text = str(midi_channel)
		else:
			if midi_channel and midi_channel.matches(event):
				var value = midi_channel.scale_val(event)
				slider.value = value
				numbox.value = value
				update_val(value)
