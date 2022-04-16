using OpenTK.Windowing.Common;

namespace ComputerGraphics.Common.Scenes;

public interface IScene : IDisposable
{
    /// <summary>
    /// Called every time this scene becomes active.
    /// </summary>
    void Load();

    /// <summary>
    /// Scene update.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    void Update(FrameEventArgs e);

    /// <summary>
    /// Scene render.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    void Render(FrameEventArgs e);

    /// <summary>
    /// Scene resize.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    void Resize(ResizeEventArgs e);
}