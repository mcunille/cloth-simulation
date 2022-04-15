using ComputerGraphics.ClothSimulation.Scenes;
using ComputerGraphics.Common.Scenes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ComputerGraphics.ClothSimulation;

/// <summary>
/// Main application window using OpenTK.
/// </summary>
public class Window : GameWindow
{
    private readonly IScene _scene;

    /// <inheritdoc/>
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
    : base(gameWindowSettings, nativeWindowSettings)
    {
        _scene = new SimpleTriangleScene();
    }

    /// <inheritdoc/>
    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Enable(EnableCap.DepthTest);

        _scene.Awake();
    }

    /// <inheritdoc/>
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _scene.Draw();

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

        _scene.Update();
    }

    /// <inheritdoc/>
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
        // Dispose of resources.
        _scene.Dispose();
        base.OnUnload();
    }
}