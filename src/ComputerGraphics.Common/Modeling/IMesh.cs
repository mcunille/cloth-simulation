using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Modeling;

public interface IMesh : IDisposable
{
    /// <summary>
    /// Draws this mesh.
    /// </summary>
    void Draw();

    /// <summary>
    /// Set 2D vertex data (x,y).
    /// </summary>
    /// <param name="vertices">The vertex data.</param>
    void SetVertexData(Vector2[] vertices);

    /// <summary>
    /// Set 3D vertex data (x,y,z).
    /// </summary>
    /// <param name="vertices">The vertex data.</param>
    void SetVertexData(Vector3[] vertices);

    /// <summary>
    /// Set color data (r,g,b).
    /// </summary>
    /// <param name="colors">The color data.</param>
    void SetColorData(Vector3[] colors);
}