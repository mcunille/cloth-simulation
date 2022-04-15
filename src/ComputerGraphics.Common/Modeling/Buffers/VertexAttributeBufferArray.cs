using ComputerGraphics.Common.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Modeling.Buffers;

public class VertexAttributeBufferArray : Buffer
{
    private readonly BufferUsageHint _usage;
    private readonly int _attributeIndex;

    public VertexAttributeBufferArray(int vertexArray, ShaderAttribute attribute, BufferUsageHint usage = BufferUsageHint.StaticDraw)
    : this(vertexArray, (int)attribute, usage)
    {
    }

    public VertexAttributeBufferArray(int vertexArray, int attributeIndex, BufferUsageHint usage = BufferUsageHint.StaticDraw)
    : base(vertexArray, BufferTarget.ArrayBuffer)
    {
        _usage = usage;
        _attributeIndex = attributeIndex;
    }

    public int Count { get; private set; }

    public void SetData(Vector2[] data) => SetData(data, size: 2, sizeInBytes: data.Length * Vector2.SizeInBytes);

    public void SetData(Vector3[] data) => SetData(data, size: 3, sizeInBytes: data.Length * Vector3.SizeInBytes);

    public virtual void SetData<T>(T[] data, int size, int sizeInBytes, int stride = 0, int offset = 0)
    where T : struct
    {
        Reset();

        ApplyArrayBufferConfiguration(() =>
        {
            GL.BufferData<T>(Target, sizeInBytes, data, _usage);
            GL.EnableVertexAttribArray(_attributeIndex);
            GL.VertexAttribPointer(_attributeIndex, size, VertexAttribPointerType.Float, false, stride, offset);
            GL.BindBuffer(Target, 0);
        });

        Count = data.Length;
    }
}