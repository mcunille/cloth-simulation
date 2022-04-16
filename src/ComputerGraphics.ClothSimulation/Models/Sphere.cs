using System.Reflection;
using ComputerGraphics.Common.Modeling;
using ComputerGraphics.Common.Shaders;
using ComputerGraphics.Common.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.ClothSimulation.Models;

public class Sphere : IModel
{
    private readonly IShader _shader;
    private readonly IMesh _mesh;
    private readonly ITexture _texture;
    private Matrix4 _modelMatrix;
    private bool _disposed;

    public Sphere(float radius, uint rings, uint sections)
    {
        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _shader = new Shader($"{assemblyPath}/Shaders/Diffuse.vert", $"{assemblyPath}/Shaders/Diffuse.frag");

        _mesh = new Mesh(primitive: PrimitiveType.Triangles);
        _texture = new Texture2D($"{assemblyPath}/Resources/container.png");
        _modelMatrix = Matrix4.Identity;
        ConfigureSphere(radius, rings, sections);
    }

    public void Draw(Matrix4 viewProjectionMatrix)
    {
        _shader.Activate();
        _shader.SetUniformMatrix4("mvpMatrix", _modelMatrix * viewProjectionMatrix);
        _texture.Activate(TextureUnit.Texture0);
        _mesh.Draw();
        _texture.Deactivate(TextureUnit.Texture0);
        _shader.Deactivate();
    }

    public void Translate(float x, float y, float z)
    {
        _modelMatrix *= Matrix4.CreateTranslation(x, y, z);
    }

    public void RotateX(float angle)
    {
        _modelMatrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
    }

    public void RotateY(float angle)
    {
        _modelMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
    }

    public void RotateZ(float angle)
    {
        _modelMatrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
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
                _shader.Dispose();
                _mesh.Dispose();
            }

            _disposed = true;
        }
    }

    private void ConfigureSphere(float radius, uint rings, uint sectors)
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

        _mesh.SetVertexData(vertexData);
        _mesh.SetTextureData(textureData);
        _mesh.SetColorData(colorData);
        _mesh.SetIndexData(indexData);
    }
}