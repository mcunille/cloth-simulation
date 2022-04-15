using ComputerGraphics.Common.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Modeling.Buffers;

public class VertexBufferArray : BufferArray
{
    private readonly BufferUsageHint _usage;

    public VertexBufferArray(int vertexArray, BufferUsageHint usage)
    : base(vertexArray)
    {
        _usage = usage;
    }

    public int Count { get; private set; } = 0;

    public void SetVertexData(Vector2[] vertices)
    {
        Reset();

        ApplyArrayBufferConfiguration(() =>
        {
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, vertices.Length * Vector2.SizeInBytes, vertices, _usage);
            GL.EnableVertexAttribArray((int)ShaderAttribute.Vertex);
            GL.VertexAttribPointer((int)ShaderAttribute.Vertex, 2, VertexAttribPointerType.Float, false, 0, 0);
        });

        Count = vertices.Length;
    }

    public void SetVertexData(Vector3[] vertices)
    {
        Reset();

        ApplyArrayBufferConfiguration(() =>
        {
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, vertices.Length * Vector3.SizeInBytes, vertices, _usage);
            GL.EnableVertexAttribArray((int)ShaderAttribute.Vertex);
            GL.VertexAttribPointer((int)ShaderAttribute.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);
        });

        Count = vertices.Length;
    }
}