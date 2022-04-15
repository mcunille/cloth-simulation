using OpenTK.Graphics.OpenGL4;

namespace ComputerGraphics.Common.Modeling.Buffers;

public abstract class BufferArray : IDisposable
{
    private readonly int _vertexArray;
    private bool _disposed;

    public BufferArray(int vertexArray)
    {
        _vertexArray = vertexArray;
    }

    ~BufferArray()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    protected int BufferID { get; private set; } = 0;

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
        GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
        configure.Invoke();
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
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