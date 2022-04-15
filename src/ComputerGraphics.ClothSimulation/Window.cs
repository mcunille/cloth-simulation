using System.Reflection;
using ComputerGraphics.Common.Meshes;
using ComputerGraphics.Common.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ComputerGraphics.ClothSimulation;

/// <summary>
/// Main application window using OpenTK.
/// Bootstrap from https://github.com/opentk/LearnOpenTK/tree/master/Chapter1/2-HelloTriangle
/// </summary>
public class Window : GameWindow
{
    private IMesh _triangleMesh = null!;
    private IShader _shader = null!;

    /// <inheritdoc/>
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
    : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    /// <inheritdoc/>
    protected override void OnLoad()
    {
        base.OnLoad();

        // Set background color.
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _triangleMesh = new Mesh(primitive: PrimitiveType.Triangles);
        _triangleMesh.SetVertexData(new Vector2[]
        {
            new Vector2(-0.5f, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(0.0f, 0.5f),
        });

        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _shader = new Shader($"{assemblyPath}/Shaders/Default.vert", $"{assemblyPath}/Shaders/Default.frag");
    }

    /// <inheritdoc/>
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Activate();
        _triangleMesh.Draw();

        SwapBuffers();
    }

    /// <inheritdoc/>
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        KeyboardState input = KeyboardState;
        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    /// <inheritdoc/>
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
        // Unbind all the resources by binding the targets to 0/null.
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        // Dispose of resources.
        _triangleMesh.Dispose();
        _shader.Dispose();

        base.OnUnload();
    }
}