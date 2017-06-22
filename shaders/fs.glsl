﻿#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 worldPos;
uniform sampler2D pixels;		// texture sampler

// shader output
out vec3 outputColor;
uniform vec3 ambientColor;
uniform vec3 lightPos;
uniform vec3 lightPos2;
uniform vec3 lightPos3;
uniform vec3 lightPos4;
uniform vec3 lightCol;
uniform vec3 lightCol2;
uniform vec3 lightCol3;
uniform vec3 lightCol4;


// fragment shader
void main()
{
	vec3 L = lightPos - worldPos.xyz;
	vec3 D = normalize(worldPos.xyz);
	float dist = L.length();
	L = normalize(L);
	vec3 result = ambientColor + lightCol;
	vec3 materialColor = texture(pixels, uv).xyz;
	vec3 R = normalize(L - 2 * dot(L, normal.xyz) * normal.xyz);
	float attenuation = 1.0f/(dist * dist);
    //vec4 Output1 = vec4(1, 1, 1, 1) * vec4(materialColor * max(0.0f, dot(L, normal.xyz)) * attenuation * result, 1);
	vec3 Output1 = ambientColor + lightCol * materialColor * dot(normal.xyz, L) + lightCol * materialColor * pow(max(0.0, dot(D, R)),20);

	vec3 L2 = lightPos2 - worldPos.xyz;
	float dist2 = L2.length();
	L2 = normalize(L2);
	vec3 lightColor2 = vec3(0, 0, 0);
	vec3 result2 = ambientColor + lightCol2;
	vec3 materialColor2 = texture(pixels, uv).xyz;
	vec3 R2 = normalize(L2 - 2 * dot(L2, normal.xyz) * normal.xyz);
	float attenuation2 = 1.0f/(dist2 * dist2);
	vec3 Output2 = ambientColor + lightCol2 * materialColor2 * dot(normal.xyz, L2) + lightCol2 * materialColor2 * pow(max(0.0, dot(D, R2)),20);

	vec3 L3 = lightPos3 - worldPos.xyz;
	float dist3 = L3.length();
	L3 = normalize(L3);
	vec3 lightColor3 = vec3(0, 0, 0);
	vec3 result3 = ambientColor + lightCol3;
	vec3 materialColor3 = texture(pixels, uv).xyz;
	vec3 R3 = normalize(L3 - 2 * dot(L3, normal.xyz) * normal.xyz);
	float attenuation3 = 1.0f/(dist3 * dist3);
	vec3 Output3 = ambientColor + lightCol3 * materialColor3 * dot(normal.xyz, L3) + lightCol3 * materialColor3 * pow(max(0.0, dot(D, R3)),20);

	vec3 L4 = lightPos4 - worldPos.xyz;
	float dist4 = L4.length();
	L4 = normalize(L4);
	vec3 result4 = ambientColor + lightCol4;
	vec3 materialColor4 = texture(pixels, uv).xyz;
	vec3 R4 = normalize(L4 - 2 * dot(L4, normal.xyz) * normal.xyz);
	float attenuation4 = 1.0f/(dist4 * dist4);
    vec3 Output4 = ambientColor + lightCol4 * materialColor4 * dot(normal.xyz, L4) + lightCol4 * materialColor4 * pow(max(0.0, dot(D, R4)),20);
    outputColor = Output1 + Output2 + Output3 + Output4;
	//outputColor.x = dot(normal.xyz, L);
	//outputColor.y = max(0.0, pow(dot(D, R), 20));
}