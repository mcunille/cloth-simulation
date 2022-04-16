using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ComputerGraphics.Common.Cameras;

public interface ICamera
{
    Vector3 Position { get; set; }
    float AspectRatio { get; set; }
    float Pitch { get; set; }
    float Yaw { get; set; }
    float Fov { get; set; }

    Matrix4 ViewProjectionMatrix { get; }

    void HandleKeyboardState(KeyboardState input, FrameEventArgs e);
}