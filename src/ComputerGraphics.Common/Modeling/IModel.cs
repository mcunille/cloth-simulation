using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Modeling;

public interface IModel : IDisposable
{
    void Draw(Matrix4 viewProjectionMatrix);

    void Translate(float x, float y, float z);

    void RotateX(float angle);
    void RotateY(float angle);
    void RotateZ(float angle);
}