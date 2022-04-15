using ComputerGraphics.Common.Modeling.Buffers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Modeling;

public class Mesh : IMesh, IDisposable
{
    private readonly PrimitiveType _primitive;

    private readonly int _vertexArrayObject;

    private readonly VertexBufferArray _vertexBuffer;
    private readonly ColorBufferArray _colorBuffer;

    private bool _disposed;

    public Mesh(PrimitiveType primitive = PrimitiveType.Points, BufferUsageHint usage = BufferUsageHint.StaticDraw)
    {
        _primitive = primitive;

        _vertexArrayObject = GL.GenVertexArray();
        _vertexBuffer = new VertexBufferArray(_vertexArrayObject, usage);
        _colorBuffer = new ColorBufferArray(_vertexArrayObject, usage);
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
        GL.DrawArrays(_primitive, 0, _vertexBuffer.Count);
        GL.BindVertexArray(0);
    }

    /// <inheritdoc/>
    public void SetVertexData(Vector2[] vertices)
    {
        _vertexBuffer.SetVertexData(vertices);
    }

    /// <inheritdoc/>
    public void SetVertexData(Vector3[] vertices)
    {
        _vertexBuffer.SetVertexData(vertices);
    }

    /// <inheritdoc/>
    public void SetColorData(Vector3[] colors)
    {
        _colorBuffer.SetColorData(colors);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects)
                _vertexBuffer.Dispose();
                _colorBuffer.Dispose();
            }

            GL.DeleteVertexArray(_vertexArrayObject);

            _disposed = true;
        }
    }
}