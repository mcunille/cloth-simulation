using ComputerGraphics.ClothSimulation.Models;
using ComputerGraphics.Common.Cameras;
using ComputerGraphics.Common.Modeling;
using ComputerGraphics.Common.Scenes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ComputerGraphics.ClothSimulation.Scenes;

public class ClothScene : IScene
{
    private readonly NativeWindow _window;
    private readonly ICamera _camera;

    private readonly IModel _sphere;

    private bool _disposed;

    public ClothScene(NativeWindow window)
    {
        _window = window;
        _camera = new Camera(
            position: new(0.0f, 0.0f, 6.0f),
            aspectRatio: _window.Size.X / (float)_window.Size.Y)
        {
            Fov = 60.0f
        };

        _sphere = new Sphere(radius: 1.0f, rings: 25, sections: 25);
    }

    public void Load()
    {
        Console.WriteLine("Starting scene: Cloth Simulation");
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
    }

    public void Update(FrameEventArgs e)
    {
        double speed = 100.0f;
        _sphere.RotateY((float)(speed * e.Time));
        _camera.HandleKeyboardState(_window.KeyboardState, e);
    }

    public void Render(FrameEventArgs e)
    {
        _sphere.Draw(viewProjectionMatrix: _camera.ViewProjectionMatrix);
    }

    public void Resize(ResizeEventArgs e)
    {
        _camera.AspectRatio = e.Size.X / (float)e.Size.Y;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _sphere.Dispose();
            }

            _disposed = true;
        }
    }
}