using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Meshes;

public interface IMesh : IDisposable
{
    /// <summary>
    /// Draws this mesh.
    /// </summary>
    void Draw();

    void SetVertexData(Vector2[] vertices);
    void SetVertexData(Vector3[] vertices);
}