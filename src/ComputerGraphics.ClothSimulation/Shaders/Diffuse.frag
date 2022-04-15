#version 450

// Color of the fragment that this shader outputs
out vec4 vFragColor;

// Interpolated texture received from the vertex shader.
in vec2 texCoords;

uniform sampler2D mainTex;

void main(void)
{
	// The final color of the fragment.
	vFragColor = texture(mainTex, texCoords);
}