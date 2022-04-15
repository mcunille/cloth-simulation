using ComputerGraphics.ClothSimulation;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

NativeWindowSettings windowSettings = new()
{
    Size = new Vector2i(800, 600),
    Title = "LearnOpenTK - Transformations",
};

using Window window = new(GameWindowSettings.Default, windowSettings);
window.Run();