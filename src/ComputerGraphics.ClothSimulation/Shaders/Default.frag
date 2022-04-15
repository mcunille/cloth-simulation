#version 450

// Color of the fragment that this shader outputs
out vec4 vFragColor;

// Interpolated color received from the vertex shader.
in vec4 color;

void main(void)
{
	// The final color of the fragment.
	vFragColor = color;
}