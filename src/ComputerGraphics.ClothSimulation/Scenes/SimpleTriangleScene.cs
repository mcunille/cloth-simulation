using System.Reflection;
using ComputerGraphics.Common.Meshes;
using ComputerGraphics.Common.Scenes;
using ComputerGraphics.Common.Shaders;
using ComputerGraphics.Common.Transforms;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.ClothSimulation.Scenes;

public class SimpleTriangleScene : IScene
{
    private readonly IMesh _triangleMesh;
    private readonly ITransform _triangleTransform;
    private readonly IShader _shader;

    private bool _disposed;

    public SimpleTriangleScene()
    {
        _triangleMesh = new Mesh(primitive: PrimitiveType.Triangles);
        _triangleMesh.SetVertexData(new Vector2[]
        {
            new Vector2(-0.5f, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(0.0f, 0.5f),
        });
        _triangleMesh.SetColorData(new Vector3[]
        {
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 1.0f),
        });

        _triangleTransform = new Transform();

        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _shader = new Shader($"{assemblyPath}/Shaders/Default.vert", $"{assemblyPath}/Shaders/Default.frag");
    }

    ~SimpleTriangleScene()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Awake()
    {
        Console.WriteLine("Starting scene: Simple Triangle");

        // Set the clear color for the frame buffer
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    }

    public void Update()
    {
        _triangleTransform.Rotate(0.0f, 90.0f * 0.01f, 0.0f, false);
    }

    public void Draw()
    {
        _shader.Activate();
        _shader.SetUniformMatrix4("mvpMatrix", _triangleTransform.ModelMatrix);

        _triangleMesh.Draw();

        _shader.Deactivate();
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
                _triangleMesh.Dispose();
                _shader.Dispose();
            }

            _disposed = true;
        }
    }
}