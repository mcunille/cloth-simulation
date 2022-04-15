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
    private int _colorBufferObject = 0;
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

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public void Draw()
    {
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(_primitive, 0, _numberOfVertices);
        GL.BindVertexArray(0);
    }

    /// <inheritdoc/>
    public void SetVertexData(Vector2[] vertices)
    {
        _vertexBufferObject = ResetBuffer(_vertexBufferObject);

        ApplyArrayBufferConfiguration(_vertexArrayObject, () =>
        {
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, vertices.Length * Vector2.SizeInBytes, vertices, _usage);

            GL.EnableVertexAttribArray((int)ShaderAttribute.Vertex);
            GL.VertexAttribPointer((int)ShaderAttribute.Vertex, 2, VertexAttribPointerType.Float, false, 0, 0);
        });

        _numberOfVertices = vertices.Length;
    }

    /// <inheritdoc/>
    public void SetVertexData(Vector3[] vertices)
    {
        _vertexBufferObject = ResetBuffer(_vertexBufferObject);

        ApplyArrayBufferConfiguration(_vertexBufferObject, () =>
        {
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, vertices.Length * Vector3.SizeInBytes, vertices, _usage);

            GL.EnableVertexAttribArray((int)ShaderAttribute.Vertex);
            GL.VertexAttribPointer((int)ShaderAttribute.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);
        });

        _numberOfVertices = vertices.Length;
    }

    /// <inheritdoc/>
    public void SetColorData(Vector3[] colors)
    {
        _colorBufferObject = ResetBuffer(_colorBufferObject);

        ApplyArrayBufferConfiguration(_colorBufferObject, () =>
        {
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, colors.Length * Vector3.SizeInBytes, colors, _usage);

            GL.EnableVertexAttribArray((int)ShaderAttribute.Color);
            GL.VertexAttribPointer((int)ShaderAttribute.Color, 3, VertexAttribPointerType.Float, false, 0, 0);
        });
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

            // Delete the color buffer if it was set.
            if (_colorBufferObject != 0)
            {
                GL.DeleteBuffer(_colorBufferObject);
            }

            _disposed = true;
        }
    }

    private void ApplyArrayBufferConfiguration(int buffer, Action configure)
    {
        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
        configure.Invoke();
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    private static int ResetBuffer(int bufferObject)
    {
        if (bufferObject != 0)
        {
            GL.DeleteBuffer(bufferObject);
        }

        return GL.GenBuffer();
    }
}