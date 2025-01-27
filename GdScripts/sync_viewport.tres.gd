extends SubViewport

# Called when the node enters the scene tree for the first time.
func _ready():
	await get_tree().process_frame
	var main_viewport : Viewport = get_parent().get_viewport()
	var update_size = func():
		self.size = main_viewport.get_visible_rect().size
	update_size.call()
	main_viewport.size_changed.connect(update_size)
	pass # Replace with function body.
