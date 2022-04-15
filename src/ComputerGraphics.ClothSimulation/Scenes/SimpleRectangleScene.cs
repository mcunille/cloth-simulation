using System.Reflection;
using ComputerGraphics.Common.Modeling;
using ComputerGraphics.Common.Scenes;
using ComputerGraphics.Common.Shaders;
using ComputerGraphics.Common.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace ComputerGraphics.ClothSimulation.Scenes;

public class SimpleRectangleScene : IScene
{
    private readonly IMesh _mesh;
    private readonly ITexture _texture;
    private readonly IShader _shader;

    private double _time;

    private Matrix4 _view;
    private Matrix4 _projection;

    private bool _disposed;

    public SimpleRectangleScene()
    {
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

        _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), 800.0f / 600.0f, 0.1f, 100.0f);
    }

    ~SimpleRectangleScene()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Awake()
    {
        Console.WriteLine("Starting scene: Simple Rectangle");

        // Set the clear color for the frame buffer
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    }

    public void Update(FrameEventArgs e)
    {
        _time += 10.0 * e.Time;
    }

    public void Draw()
    {
        Matrix4 model = Matrix4.Identity
            * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time))
            * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time))
            * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(_time));

        _texture.Activate(TextureUnit.Texture0);
        _shader.Activate();
        _shader.SetUniformMatrix4("mvpMatrix", model * _view * _projection);

        _mesh.Draw();

        _shader.Deactivate();
        _texture.Deactivate(TextureUnit.Texture0);
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