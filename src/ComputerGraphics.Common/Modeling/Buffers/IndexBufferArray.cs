using OpenTK.Graphics.OpenGL4;

namespace ComputerGraphics.Common.Modeling.Buffers;

public class IndexBufferArray : Buffer
{
    private readonly BufferUsageHint _usage;

    public IndexBufferArray(int vertexArray, BufferUsageHint usage)
    : base(vertexArray, BufferTarget.ElementArrayBuffer)
    {
        _usage = usage;
    }

    public int Count { get; private set; } = 0;

    public void SetIndexData(uint[] indices)
    {
        Reset();

        ApplyArrayBufferConfiguration(() =>
        {
            GL.BufferData(Target, indices.Length * sizeof(uint), indices, _usage);
        });

        Count = indices.Length;
    }
}