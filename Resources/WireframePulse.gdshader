shader_type spatial;
render_mode unshaded,wireframe,cull_disabled;

uniform float outline_width : hint_range(0.0, 10.0, 0.1) = 3.0;


void vertex() {
	vec4 clip_position = PROJECTION_MATRIX * (MODELVIEW_MATRIX * vec4(VERTEX, 1.0));
	vec3 clip_normal = mat3(PROJECTION_MATRIX) * (mat3(MODELVIEW_MATRIX) * NORMAL);
	
	vec2 offset = normalize(clip_normal.xy) / VIEWPORT_SIZE * 
		clip_position.w * outline_width * 2.0;
	
	
	
	clip_position.xy += offset;
	
	POSITION = clip_position;
}


void fragment() {
	float sint = sin(TIME * 2.0f);
	sint = pow(sint, 2.0f);
	sint = 1.0f - sint;
	sint = pow(sint, 1.0f / 2.0f);
	sint = 1.0f - sint;
	
	ALBEDO = vec3(0, abs(sint), 0);
}