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

public class ClothScene : IScene
{
    private readonly NativeWindow _window;
    private readonly ICamera _camera;
    private readonly IShader _colorShader;
    private readonly IShader _textureShader;

    private readonly IMesh _sphereMesh;
    private readonly ITexture _sphereTexture;
    private Matrix4 _sphereModelMatrix;

    private bool _disposed;

    public ClothScene(NativeWindow window)
    {
        _window = window;
        _camera = new Camera(
            position: new(0.0f, 2.0f, 6.0f),
            aspectRatio: _window.Size.X / (float)_window.Size.Y)
        {
            Fov = 60.0f
        };

        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _colorShader = new Shader($"{assemblyPath}/Shaders/Default.vert", $"{assemblyPath}/Shaders/Default.frag");
        _textureShader = new Shader($"{assemblyPath}/Shaders/Diffuse.vert", $"{assemblyPath}/Shaders/Diffuse.frag");

        _sphereMesh = new Mesh(primitive: PrimitiveType.Triangles);
        _sphereTexture = new Texture2D($"{assemblyPath}/Resources/container.png");
        _sphereModelMatrix = Matrix4.Identity * Matrix4.CreateTranslation(0.0f, 2.0f, 0.0f);
        ConfigureSphere(radius: 1.0f, rings: 25, sectors: 25);
    }

    ~ClothScene()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Load()
    {
        Console.WriteLine("Starting scene: Cloth Simulation");
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
    }

    public void Update(FrameEventArgs e)
    {
        double speed = 100.0f;
        _sphereModelMatrix *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(speed * e.Time));

        _camera.HandleKeyboardState(_window.KeyboardState, e);
    }

    public void Render(FrameEventArgs e)
    {
        // Draw sphere
        _textureShader.Activate();
        _textureShader.SetUniformMatrix4("mvpMatrix", _sphereModelMatrix * _camera.ViewMatrix * _camera.ProjectionMatrix);
        _sphereTexture.Activate(TextureUnit.Texture0);
        _sphereMesh.Draw();
        _sphereTexture.Deactivate(TextureUnit.Texture0);
        _textureShader.Deactivate();
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
                _colorShader.Dispose();
            }

            _disposed = true;
        }
    }

    private void ConfigureSphere(float radius, int rings, int sectors)
    {
        List<Vector3> vertexData = new();
        List<Vector2> textureData = new();
        List<Vector3> colorData = new();
        List<uint> indexData = new();

        float rStep = 1.0f / (rings - 1);
        float sStep = 1.0f / (sectors - 1);

        // Create a sphere vertices
        for (int r = 0; r < rings; r++)
        {
            for (int s = 0; s < sectors; s++)
            {
                float y = (float)MathHelper.Sin(-MathHelper.PiOver2 + MathHelper.Pi * r * rStep);
                float x = (float)(MathHelper.Cos(2 * MathHelper.Pi * s * sStep) * MathHelper.Sin(MathHelper.Pi * r * rStep));
                float z = (float)(MathHelper.Sin(2 * MathHelper.Pi * s * sStep) * MathHelper.Sin(MathHelper.Pi * r * rStep));

                vertexData.Add(new(x * radius, y * radius, z * radius));
                textureData.Add(new(s * sStep, r * rStep));
                colorData.Add(new(0.0f, 0.0f, 0.0f));
            }
        }

        // Create sphere elements
        for (int r = 0; r < rings - 1; r++)
        {
            for (int s = 0; s < sectors - 1; s++)
            {
                uint tl = (uint)(r * sectors + s);
                uint tr = (uint)(r * sectors + (s + 1));
                uint br = (uint)((r + 1) * sectors + (s + 1));
                uint bl = (uint)((r + 1) * sectors + s);

                indexData.AddRange(new uint[] { tl, tr, bl });
                indexData.AddRange(new uint[] { tr, bl, br });
            }
        }

        _sphereMesh.SetVertexData(vertexData);
        _sphereMesh.SetTextureData(textureData);
        _sphereMesh.SetColorData(colorData);
        _sphereMesh.SetIndexData(indexData);
    }

    private static class AxisConstraints
    {
        public const float XMin = -100.0f;
        public const float XMax = 100.0f;
        public const float YMin = -100.0f;
        public const float YMax = 100.0f;
        public const float ZMin = -100.0f;
        public const float ZMax = 100.0f;
    }
}