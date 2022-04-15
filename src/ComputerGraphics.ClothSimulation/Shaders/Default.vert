#version 450

// Incoming position per vertex
in vec4 vVertex;
// Incoming color per vertex
in vec4 vColor;

// Data sent to the fragment shader
out vec4 color;

void main(void)
{
	// Set the color of this vertex so it can be interpolated by the fragment shader
	color = vColor;

	// Compute the position of the current vertex
	gl_Position = vVertex;
}
