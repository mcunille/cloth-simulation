using System.Reflection;
using ComputerGraphics.Common.Cameras;
using ComputerGraphics.Common.Modeling;
using ComputerGraphics.Common.Scenes;
using ComputerGraphics.Common.Shaders;
using ComputerGraphics.Common.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ComputerGraphics.ClothSimulation.Scenes;

public class SimpleContainerScene : IScene
{
    private readonly NativeWindow _window;

    private readonly IMesh _mesh;
    private readonly ITexture _texture;
    private readonly IShader _shader;
    private readonly ICamera _camera;

    private double _time;

    private bool _disposed;

    public SimpleContainerScene(NativeWindow window)
    {
        _window = window;

        _mesh = new Mesh(primitive: PrimitiveType.Triangles);
        _mesh.Set3DVertexData(new float[]
        {
            -0.5f, -0.5f, -0.5f,
            0.5f, -0.5f, -0.5f,
            0.5f,  0.5f, -0.5f,
            0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f, -0.5f,  0.5f,
            0.5f, -0.5f,  0.5f,
            0.5f,  0.5f,  0.5f,
            0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,

            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,

            0.5f,  0.5f,  0.5f,
            0.5f,  0.5f, -0.5f,
            0.5f, -0.5f, -0.5f,
            0.5f, -0.5f, -0.5f,
            0.5f, -0.5f,  0.5f,
            0.5f,  0.5f,  0.5f,

            -0.5f, -0.5f, -0.5f,
            0.5f, -0.5f, -0.5f,
            0.5f, -0.5f,  0.5f,
            0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f,  0.5f, -0.5f,
            0.5f,  0.5f, -0.5f,
            0.5f,  0.5f,  0.5f,
            0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f,
        });
        _mesh.Set2DTextureData(new float[]
        {
            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f,
            1.0f, 1.0f,
            0.0f, 1.0f,
            0.0f, 0.0f,

            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f,
            1.0f, 1.0f,
            0.0f, 1.0f,
            0.0f, 0.0f,

            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f,
            1.0f, 1.0f,
            0.0f, 1.0f,
            0.0f, 0.0f,

            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f,
            1.0f, 1.0f,
            0.0f, 1.0f,
            0.0f, 0.0f,

            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f,
            1.0f, 1.0f,
            0.0f, 1.0f,
            0.0f, 0.0f,

            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f,
            1.0f, 1.0f,
            0.0f, 1.0f,
            0.0f, 0.0f,
        });

        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _shader = new Shader($"{assemblyPath}/Shaders/Diffuse.vert", $"{assemblyPath}/Shaders/Diffuse.frag");
        _texture = new Texture2D($"{assemblyPath}/Resources/container.png");

        _camera = new Camera(
            position: Vector3.UnitZ * 3, // Move backwards
            aspectRatio: _window.Size.X / (float)_window.Size.Y);
    }

    ~SimpleContainerScene()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Load()
    {
        Console.WriteLine("Starting scene: Simple Rectangle");

        // Set the clear color for the frame buffer
        GL.ClearColor(Color4.SkyBlue);
    }

    public void Update(FrameEventArgs e)
    {
        double speed = 10.0;
        _time += speed * e.Time;

        _camera.HandleKeyboardState(_window.KeyboardState, e);
    }

    public void Render(FrameEventArgs e)
    {
        Matrix4 model = Matrix4.Identity
            * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time))
            * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time))
            * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(_time));

        _texture.Activate(TextureUnit.Texture0);
        _shader.Activate();
        _shader.SetUniformMatrix4("mvpMatrix", model * _camera.ViewMatrix * _camera.ProjectionMatrix);

        _mesh.Draw();

        _shader.Deactivate();
        _texture.Deactivate(TextureUnit.Texture0);
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
                // Dispose managed state (managed objects)
                _mesh.Dispose();
                _shader.Dispose();
            }

            _disposed = true;
        }
    }
}