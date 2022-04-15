using ComputerGraphics.Common.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Meshes;

public class Mesh : IMesh, IDisposable
{
    private readonly PrimitiveType _primitive;
    private readonly BufferUsageHint _usage;

    private readonly int _vertexArrayObject;
    private int _vertexBufferObject = 0;
    private int _numberOfVertices = 0;
    private bool _disposed;

    public Mesh(PrimitiveType primitive = PrimitiveType.Points, BufferUsageHint usage = BufferUsageHint.StaticDraw)
    {
        _primitive = primitive;
        _usage = usage;
        _vertexArrayObject = GL.GenVertexArray();
    }

    ~Mesh()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Draw()
    {
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(_primitive, 0, _numberOfVertices);
        GL.BindVertexArray(0);
    }

    public void SetVertexData(Vector2[] vertices)
    {
        if (_vertexBufferObject != 0)
        {
            // Delete existing buffer
            GL.DeleteBuffer(_vertexBufferObject);
        }

        _vertexBufferObject = GL.GenBuffer();

        GL.BindVertexArray(_vertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, vertices.Length * Vector2.SizeInBytes, vertices, _usage);

        GL.EnableVertexAttribArray((int)ShaderAttribute.Vertex);
        GL.VertexAttribPointer((int)ShaderAttribute.Vertex, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        _numberOfVertices = vertices.Length;

        GL.BindVertexArray(0);
    }

    public void SetVertexData(Vector3[] vertices)
    {
        if (_vertexBufferObject != 0)
        {
            // Delete existing buffer
            GL.DeleteBuffer(_vertexBufferObject);
        }

        _vertexBufferObject = GL.GenBuffer();

        GL.BindVertexArray(_vertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, vertices.Length * Vector3.SizeInBytes, vertices, _usage);

        GL.EnableVertexAttribArray((int)ShaderAttribute.Vertex);
        GL.VertexAttribPointer((int)ShaderAttribute.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        _numberOfVertices = vertices.Length;

        GL.BindVertexArray(0);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects)
                // No-op
            }

            GL.DeleteVertexArray(_vertexArrayObject);

            // Delete the vertex buffer if it was set.
            if (_vertexBufferObject != 0)
            {
                GL.DeleteBuffer(_vertexBufferObject);
            }

            _disposed = true;
        }
    }
}