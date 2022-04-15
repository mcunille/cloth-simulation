using System.Reflection;
using ComputerGraphics.Common.Modeling;
using ComputerGraphics.Common.Scenes;
using ComputerGraphics.Common.Shaders;
using ComputerGraphics.Common.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.ClothSimulation.Scenes;

public class SimpleRectangleScene : IScene
{
    private readonly IMesh _mesh;
    private readonly ITexture _texture;
    private readonly IShader _shader;

    private bool _disposed;

    public SimpleRectangleScene()
    {
        _mesh = new Mesh(primitive: PrimitiveType.Triangles);
        _mesh.SetVertexData(new Vector2[]
        {
            new Vector2(0.5f, 0.5f),    // top right
            new Vector2(0.5f, -0.5f),   // bottom right
            new Vector2(-0.5f, -0.5f),  // bottom left
            new Vector2(-0.5f, 0.5f),   // top left
        });
        _mesh.SetIndexData(new uint[]
        {
            0, 1, 3,    // first triangle
            1, 2, 3,    // second triangle
        });
        _mesh.SetTextureData(new Vector2[]
        {
            new Vector2(1.0f, 1.0f),    // top right
            new Vector2(1.0f, 0.0f),    // bottom right
            new Vector2(0.0f, 0.0f),    // bottom left
            new Vector2(0.0f, 1.0f),    // top left
        });

        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _shader = new Shader($"{assemblyPath}/Shaders/Diffuse.vert", $"{assemblyPath}/Shaders/Diffuse.frag");
        _texture = new Texture2D($"{assemblyPath}/Resources/container.png");
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

    public void Update()
    {
        //_camera.Yaw(90.0f * 0.01f);
    }

    public void Draw()
    {
        _texture.Activate(TextureUnit.Texture0);
        _shader.Activate();
        _shader.SetUniformMatrix4("mvpMatrix", Matrix4.Identity);

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