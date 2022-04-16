using ComputerGraphics.Common.Modeling.Buffers;
using ComputerGraphics.Common.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Modeling;

public class Mesh : IMesh, IDisposable
{
    private readonly PrimitiveType _primitive;

    private readonly int _vertexArrayObject;

    private readonly VertexAttributeBufferArray _vertexBuffer;
    private readonly VertexAttributeBufferArray _colorBuffer;
    private readonly VertexAttributeBufferArray _textureBuffer;
    private readonly IndexBufferArray _indexBuffer;

    private bool _disposed;

    public Mesh(PrimitiveType primitive = PrimitiveType.Points, BufferUsageHint usage = BufferUsageHint.StaticDraw)
    {
        _primitive = primitive;

        _vertexArrayObject = GL.GenVertexArray();
        _vertexBuffer = new VertexAttributeBufferArray(_vertexArrayObject, ShaderAttribute.Vertex, usage);
        _colorBuffer = new VertexAttributeBufferArray(_vertexArrayObject, ShaderAttribute.Color, usage);
        _textureBuffer = new VertexAttributeBufferArray(_vertexArrayObject, ShaderAttribute.Texture, usage);
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
    public void SetVertexData(IEnumerable<Vector2> vertices) => _vertexBuffer.SetData(vertices);

    /// <inheritdoc/>
    public void SetVertexData(IEnumerable<Vector3> vertices) => _vertexBuffer.SetData(vertices);

    /// <inheritdoc/>
    public void Set2DVertexData(IEnumerable<float> vertices) => SetData(_vertexBuffer, vertices, size: 2);

    /// <inheritdoc/>
    public void Set3DVertexData(IEnumerable<float> vertices) => SetData(_vertexBuffer, vertices, size: 3);

    /// <inheritdoc/>
    public void SetColorData(IEnumerable<Vector3> colors) => _colorBuffer.SetData(colors);

    /// <inheritdoc/>
    public void SetTextureData(IEnumerable<Vector2> coordinates) => _textureBuffer.SetData(coordinates);

    /// <inheritdoc/>
    public void Set2DTextureData(IEnumerable<float> coordinates) => SetData(_textureBuffer, coordinates, size: 2);

    /// <inheritdoc/>
    public void SetIndexData(IEnumerable<uint> indices) => _indexBuffer.SetIndexData(indices);

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

    private static void SetData(VertexAttributeBufferArray buffer, IEnumerable<float> data, int size)
    {
        buffer.SetData<float>(data, size, data.Count() * sizeof(float), stride: size * sizeof(float));
    }
}