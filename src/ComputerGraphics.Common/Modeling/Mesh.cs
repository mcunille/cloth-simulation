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
    private readonly TextureBufferArray _textureBuffer;
    private readonly IndexBufferArray _indexBuffer;

    private bool _disposed;

    public Mesh(PrimitiveType primitive = PrimitiveType.Points, BufferUsageHint usage = BufferUsageHint.StaticDraw)
    {
        _primitive = primitive;

        _vertexArrayObject = GL.GenVertexArray();
        _vertexBuffer = new VertexBufferArray(_vertexArrayObject, usage);
        _colorBuffer = new ColorBufferArray(_vertexArrayObject, usage);
        _textureBuffer = new TextureBufferArray(_vertexArrayObject, usage);
        _indexBuffer = new IndexBufferArray(_vertexArrayObject, usage);
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

        if (_indexBuffer.Count > 0)
        {
            GL.DrawElements(_primitive, _indexBuffer.Count, DrawElementsType.UnsignedInt, 0);
        }
        else
        {
            GL.DrawArrays(_primitive, 0, _vertexBuffer.Count);
        }

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

    /// <inheritdoc/>
    public void SetTextureData(Vector2[] coordinates)
    {
        _textureBuffer.SetTextureData(coordinates);
    }

    /// <inheritdoc/>
    public void SetIndexData(uint[] indices)
    {
        _indexBuffer.SetIndexData(indices);
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
                _textureBuffer.Dispose();
                _indexBuffer.Dispose();
            }

            GL.DeleteVertexArray(_vertexArrayObject);

            _disposed = true;
        }
    }
}