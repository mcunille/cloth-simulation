using ComputerGraphics.Common.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Modeling.Buffers;

public class TextureBufferArray : Buffer
{
    private readonly BufferUsageHint _usage;

    public TextureBufferArray(int vertexArray, BufferUsageHint usage)
    : base(vertexArray, BufferTarget.ArrayBuffer)
    {
        _usage = usage;
    }

    public void SetTextureData(Vector2[] coordinates)
    {
        Reset();

        ApplyArrayBufferConfiguration(() =>
        {
            GL.BufferData<Vector2>(Target, coordinates.Length * Vector2.SizeInBytes, coordinates, _usage);
            GL.EnableVertexAttribArray((int)ShaderAttribute.Texture);
            GL.VertexAttribPointer((int)ShaderAttribute.Texture, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(Target, 0);
        });
    }
}