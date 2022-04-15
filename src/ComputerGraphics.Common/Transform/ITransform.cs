using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Transform;

public interface ITransform
{
    Matrix4 ModelMatrix { get; }

    Vector3 Position { get; set; }

    Vector3 Scale { get; set; }

    Quaternion Rotation { get; set; }

    void Translate(float x, float y, float z, bool world);
    void Translate(Vector3 translation, bool world);

    void MoveForward(float delta, bool world);
    void MoveUp(float delta, bool world);
    void MoveRight(float delta, bool world);

    void Rotate(float x, float y, float z, bool world);
    void Rotate(Vector3 degrees, bool world);
}