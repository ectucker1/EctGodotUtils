extends CheckButton


func bind(model: LiveValuesModel, val: LVBool) -> void:
	button_pressed = val.get_val()
	pressed.connect(func(): val.set_val(button_pressed))
