using System.Reflection;
using ComputerGraphics.Common.Shaders;
using OpenTK.Graphics.OpenGL4;
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
    private readonly float[] _vertices =
    {
        -0.5f, -0.5f, 0.0f, // Bottom-left vertex
         0.5f, -0.5f, 0.0f, // Bottom-right vertex
         0.0f,  0.5f, 0.0f  // Top vertex
    };

    private int _vertexBufferObject;
    private int _vertexArrayObject;

    // Shader will be ininitialied on load and won't be null thereafter.
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

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        GL.EnableVertexAttribArray(0);

        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _shader = new Shader($"{assemblyPath}/Shaders/Default.vert", $"{assemblyPath}/Shaders/Default.frag");
        _shader.Activate();
    }

    /// <inheritdoc/>
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Activate();

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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

        // Delete all the resources.
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);

        // Dispose of shader resources.
        _shader.Dispose();

        base.OnUnload();
    }
}