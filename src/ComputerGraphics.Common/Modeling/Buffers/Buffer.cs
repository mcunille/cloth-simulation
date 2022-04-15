using OpenTK.Graphics.OpenGL4;

namespace ComputerGraphics.Common.Modeling.Buffers;

public abstract class Buffer : IDisposable
{
    private readonly int _vertexArray;

    private bool _disposed;

    public Buffer(int vertexArray, BufferTarget target)
    {
        _vertexArray = vertexArray;
        Target = target;
    }

    ~Buffer()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public int BufferID { get; private set; } = 0;

    protected BufferTarget Target { get; private set; }

    public void Reset()
    {
        if (BufferID != 0)
        {
            GL.DeleteBuffer(BufferID);
        }

        BufferID = GL.GenBuffer();
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected void ApplyArrayBufferConfiguration(Action configure)
    {
        GL.BindVertexArray(_vertexArray);
        GL.BindBuffer(Target, BufferID);
        configure.Invoke();
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

            if (BufferID != 0)
            {
                GL.DeleteBuffer(BufferID);
            }

            _disposed = true;
        }
    }
}