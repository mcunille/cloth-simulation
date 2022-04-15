using ComputerGraphics.Common.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Modeling.Buffers;

public class ColorBufferArray : Buffer
{
    private readonly BufferUsageHint _usage;

    public ColorBufferArray(int vertexArray, BufferUsageHint usage)
    : base(vertexArray, BufferTarget.ArrayBuffer)
    {
        _usage = usage;
    }

    public void SetColorData(Vector3[] colors)
    {
        Reset();

        ApplyArrayBufferConfiguration(() =>
        {
            GL.BufferData<Vector3>(Target, colors.Length * Vector3.SizeInBytes, colors, _usage);
            GL.EnableVertexAttribArray((int)ShaderAttribute.Color);
            GL.VertexAttribPointer((int)ShaderAttribute.Color, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(Target, 0);
        });
    }
}