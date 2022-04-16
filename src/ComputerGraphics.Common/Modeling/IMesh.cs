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
    void SetVertexData(IEnumerable<Vector2> vertices);

    /// <summary>
    /// Set 3D vertex data (x,y,z).
    /// </summary>
    /// <param name="vertices">The vertex data.</param>
    void SetVertexData(IEnumerable<Vector3> vertices);

    /// <summary>
    /// Set 2D vertex data (x,y).
    /// </summary>
    /// <param name="vertices">The vertex data.</param>
    void Set2DVertexData(IEnumerable<float> vertices);

    /// <summary>
    /// Set 3D vertex data (x,y,z).
    /// </summary>
    /// <param name="vertices">The vertex data.</param>
    void Set3DVertexData(IEnumerable<float> vertices);

    /// <summary>
    /// Set color data (r,g,b).
    /// </summary>
    /// <param name="colors">The color data.</param>
    void SetColorData(IEnumerable<Vector3> colors);

    /// <summary>
    /// Set 2D texture coordinates (x,y).
    /// </summary>
    /// <param name="vertices">The texture coordinates.</param>
    void SetTextureData(IEnumerable<Vector2> coordinates);

    /// <summary>
    /// Set 2D texture coordinates (x,y).
    /// </summary>
    /// <param name="vertices">The texture coordinates.</param>
    void Set2DTextureData(IEnumerable<float> coordinates);

    void SetIndexData(IEnumerable<uint> indices);
}