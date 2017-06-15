#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 worldPos;
uniform sampler2D pixels;		// texture sampler
uniform vec4 ambientColor;

// shader output
out vec4 outputColor;
uniform vec3 lightPos;
uniform vec3 lightPos2;

// fragment shader
void main()
{
	vec3 L = lightPos - worldPos.xyz;
	float dist = L.length();
	L = normalize(L);
	vec3 lightColor = vec3(10, 10, 8);
	vec3 materialColor = texture(pixels, uv).xyz;
	float attenuation = 1.0f/(dist * dist);
    //outputColor = vec4(1, 1, 1, 1) * vec4(materialColor * max(0.0f, dot(L, normal.xyz)) * attenuation * lightColor, 1);

	vec3 L2 = lightPos2 - worldPos.xyz;
	float dist2 = L2.length();
	L2 = normalize(L2);
	vec3 lightColor2 = vec3(10, 10, 8);
	vec3 materialColor2 = texture(pixels, uv).xyz;
	float attenuation2 = 1.0f/(dist2 * dist2);
    outputColor = vec4(1, 1, 1, 1) * vec4(materialColor2 * max(0.0f, dot(L2, normal.xyz)) * attenuation2 * lightColor2, 1) + vec4(1, 1, 1, 1) * vec4(materialColor * max(0.0f, dot(L, normal.xyz)) * attenuation * lightColor, 1);
}