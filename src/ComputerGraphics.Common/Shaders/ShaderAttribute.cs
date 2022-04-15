namespace ComputerGraphics.Common.Shaders;

/// <summary>
/// Well-known shader attributes.
/// </summary>
public enum ShaderAttribute
{
    Vertex = 0,
    Color,
    Normal,
    Texture,
}

/// <summary>
/// Shader attribute extension methods.
/// </summary>
public static class ShaderAttributeExtensions
{
    /// <summary>
    /// Gets the attribute location name.
    /// </summary>
    /// <param name="attribute">The shader attribute.</param>
    /// <returns>The location name.</returns>
    public static string GetLocation(this ShaderAttribute attribute)
    {
        return attribute switch
        {
            ShaderAttribute.Vertex => "vVertex",
            ShaderAttribute.Color => "vColor",
            ShaderAttribute.Normal => "vNormal",
            ShaderAttribute.Texture => "vTexture",
            _ => throw new NotImplementedException(),
        };
    }
}