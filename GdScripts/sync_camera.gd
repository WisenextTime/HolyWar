extends Camera3D

@export var Target : Camera3D

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	transform = Target.transform
	h_offset = Target.h_offset
	v_offset = Target.v_offset
	pass
