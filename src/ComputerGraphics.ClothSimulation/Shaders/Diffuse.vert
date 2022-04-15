#version 450

// Incoming position per vertex
in vec4 vVertex;
// Incoming texture coordinates per vertex
in vec2 vTexture;

// Data sent to the fragment shader
out vec2 texCoords;

// Uniform matrix, ModelViewProjection matrix
uniform mat4 mvpMatrix;

void main(void)
{
	// Set the texture of this vertex so it can be interpolated by the fragment shader
	texCoords = vTexture;

	// Compute the position of the current vertex
	gl_Position = mvpMatrix * vVertex;
}