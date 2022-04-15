using OpenTK.Windowing.Common;

namespace ComputerGraphics.Common.Scenes;

public interface IScene : IDisposable
{
    /// <summary>
    /// Called every time this scene becomes active.
    /// </summary>
    void Awake();

    /// <summary>
    /// Scene render.
    /// </summary>
    void Draw();

    /// <summary>
    /// Called every time this scene becomes inactive.
    /// </summary>
    void Sleep() { }

    /// <summary>
    /// Reset the state of the scene.
    /// </summary>
    void Reset() { }

    /// <summary>
    /// Scene update.
    /// </summary>
    void Update(FrameEventArgs e) { }
}