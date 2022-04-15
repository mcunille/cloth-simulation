using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Shaders;

public interface IShader : IDisposable
{
    void Activate();
    void Deactivate();

    void SetUniformInt(string name, int data);
    void SetUniformFloat(string name, float data);
    void SetUniformMatrix4(string name, Matrix4 data);
    void SetUniformVector3(string name, Vector3 data);
}