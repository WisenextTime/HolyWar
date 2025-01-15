shader_type spatial;
render_mode unshaded;

#define SQRT3_DIV2 0.8660254037844

uniform sampler2D SCREEN_TEXTURE: hint_screen_texture;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture;

uniform sampler2D TileMaskLayer;
uniform vec4 LineColor: source_color = vec4(0.2,0.2,0.2,0.8);
uniform float LineWidth: hint_range(0.0, 0.1) = 0.01;

void fragment() {
	vec4 color = LineColor;
	float depth = textureLod(DEPTH_TEXTURE, SCREEN_UV, 0.0).r;
	vec4 upos = INV_VIEW_MATRIX * INV_PROJECTION_MATRIX * vec4(SCREEN_UV * 2.0 - 1.0, depth, 1.0);
	vec3 pos = upos.xyz/upos.w;
	float width = asin(LineWidth);
	
	vec3 normal_axis = vec3(pos.x, (pos.x/2.0+SQRT3_DIV2*pos.z), (pos.x/2.0-SQRT3_DIV2*pos.z));
	vec3 sin_normal = cos(normal_axis/SQRT3_DIV2*PI);
	vec3 line_axis = vec3(pos.z, (pos.z/2.0-SQRT3_DIV2*pos.x), (pos.z/2.0+SQRT3_DIV2*pos.x));
	vec3 sin_line = -cos(line_axis*2.0/3.0*PI);
	vec3 sinv = step(1.0-width, abs(sin_normal)) * sign(sin_normal) * sin_line;
	
	// vec3 sin_nabs = abs(sin_normal);
	color.a *= step(0.5, max(max(sinv.x,sinv.y),sinv.z));
	
	color.a *= (1. - texture(TileMaskLayer, SCREEN_UV).a);
	color.a *= step(0,pos.y);
	ALBEDO = color.rgb;
	ALPHA = color.a;
}

