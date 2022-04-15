using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Shaders;

public class Shader : IShader
{
    private readonly int _shaderID;

    private readonly Dictionary<string, int> _uniformLocations = new();

    private bool _disposed;

    /// <summary>
    /// Initializes a shader program with vertex and fragment shaders.
    /// </summary>
    /// <param name="vertPath">The path to the source for the vertex shader.</param>
    /// <param name="fragPath">The path to the source for the fragment shader.</param>
    public Shader(string vertPath, string fragPath)
    {
        _shaderID = GL.CreateProgram();

        LoadShader(vertPath, ShaderType.VertexShader);
        LoadShader(fragPath, ShaderType.FragmentShader);

        LinkProgram();

        GetUniformLocations();
    }

    ~Shader()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    /// <inheritdoc/>
    public void Activate()
    {
        GL.UseProgram(_shaderID);
    }

    /// <inheritdoc/>
    public void SetUniformInt(string name, int data)
    {
        GL.UseProgram(_shaderID);
        GL.Uniform1(_uniformLocations[name], data);
    }

    /// <inheritdoc/>
    public void SetUniformFloat(string name, float data)
    {
        GL.UseProgram(_shaderID);
        GL.Uniform1(_uniformLocations[name], data);
    }

    /// <inheritdoc/>
    public void SetUniformMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(_shaderID);
        GL.UniformMatrix4(_uniformLocations[name], transpose: true, matrix: ref data);
    }

    /// <inheritdoc/>
    public void SetUniformVector3(string name, Vector3 data)
    {
        GL.UseProgram(_shaderID);
        GL.Uniform3(_uniformLocations[name], data);
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
                // No-op
            }

            GL.DeleteProgram(_shaderID);
            _disposed = true;
        }
    }

    private void LoadShader(string path, ShaderType type)
    {
        var source = File.ReadAllText(path);
        var shader = GL.CreateShader(type);

        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        // Check for compilation errors.
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        if (code != (int)All.True)
        {
            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }

        GL.AttachShader(_shaderID, shader);

        // The handler is no longer needed as it will be linked to the program, mark for deletion.
        GL.DeleteShader(shader);
    }

    private void LinkProgram()
    {
        // Set default attributes
        SetAttribute(ShaderAttribute.Vertex);
        SetAttribute(ShaderAttribute.Color);
        SetAttribute(ShaderAttribute.Normal);
        SetAttribute(ShaderAttribute.Texture);

        GL.LinkProgram(_shaderID);

        // Check for linking errors
        GL.GetProgram(_shaderID, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            throw new Exception($"Error occurred whilst linking Program({_shaderID})");
        }
    }

    private void SetAttribute(ShaderAttribute attribute)
    {
        GL.BindAttribLocation(_shaderID, (int)attribute, attribute.GetLocation());
    }

    private void GetUniformLocations()
    {
        // First, we have to get the number of active uniforms in the shader.
        GL.GetProgram(_shaderID, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        // Loop over all the uniforms,
        for (var i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = GL.GetActiveUniform(_shaderID, i, out _, out _);

            // get the location,
            var location = GL.GetUniformLocation(_shaderID, key);

            // and then add it to the dictionary.
            _uniformLocations.Add(key, location);
        }
    }
}