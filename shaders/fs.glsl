#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 worldPos;
uniform sampler2D pixels;		// texture sampler

// shader output
out vec3 outputColor;
uniform vec3 ambientColor;
uniform vec3 lightPos[4];
uniform vec3 lightCol[4];
uniform vec3 specCol[4];

// fragment shader
void main()
{
	vec3 totalDiffuse = vec3(0.0);
	vec3 totalSpecular = vec3(0.0);
	
	for(int i = 0; i < 4; i++)
	{
		vec3 L = lightPos[i] - worldPos.xyz;
		vec3 D = normalize(worldPos.xyz);
		float dist = L.length();
		L = normalize(L);;
		vec3 materialColor = texture(pixels, uv).xyz;
		vec3 R = normalize(L - 2 * dot(L, normal.xyz) * normal.xyz);
		float attenuation = 1.0f/(dist * dist);
		totalDiffuse = totalDiffuse + lightCol[i] * materialColor * dot(normal.xyz, L);
		totalSpecular = totalSpecular + specCol[i] * materialColor * pow(max(0.0, dot(D, R)),20);
	}
	outputColor = ambientColor + totalDiffuse + totalSpecular;

	
	//adding up total lighting; 
}