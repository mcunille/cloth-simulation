using OpenTK.Graphics.OpenGL4;

namespace ComputerGraphics.Common.Textures;

public interface ITexture
{
    void Activate(TextureUnit unit);
    void Deactivate(TextureUnit unit);
}