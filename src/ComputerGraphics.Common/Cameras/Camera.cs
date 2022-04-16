using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ComputerGraphics.Common.Cameras;

public class Camera : ICamera
{
    private const float MovementSpeed = 2.0f;
    private const float RotationSpeed = 20.0f;

    private Vector3 _front = -Vector3.UnitZ;
    private Vector3 _up = Vector3.UnitY;
    private Vector3 _right = Vector3.UnitX;

    /// <summary>
    /// Rotation around the X axis in radians.
    /// </summary>
    private float _pitch;

    /// <summary>
    /// Rotation around the Y axis in radians.
    /// </summary>
    /// <remarks>
    /// Initialize with a 90 degree rotation.
    /// </remarks>
    private float _yaw = -MathHelper.PiOver2;

    /// <summary>
    /// The camera field of view in radians.
    /// </summary>
    private float _fov = MathHelper.PiOver2;

    public Camera(Vector3 position, float aspectRatio)
    {
        Position = position;
        AspectRatio = aspectRatio;
    }

    public Vector3 Position { get; set; }

    public float AspectRatio { get; set; }

    /// <summary>
    /// Gets or sets the camera rotation in the X axis in degrees.
    /// </summary>
    public float Pitch
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            // Clamp to avoid camera from going upside down.
            float angle = MathHelper.Clamp(value, -89f, 89f);
            _pitch = MathHelper.DegreesToRadians(angle);
            UpdateLocalVectors();
        }
    }

    /// <summary>
    /// Gets or sets the camera rotation in the Y axis in degrees.
    /// </summary>
    public float Yaw
    {
        get => MathHelper.RadiansToDegrees(_yaw);
        set
        {
            _yaw = MathHelper.DegreesToRadians(value);
            UpdateLocalVectors();
        }
    }

    /// <summary>
    /// Gets or sets the camera field of view in degrees.
    /// </summary>
    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            float angle = MathHelper.Clamp(value, 1f, 90f);
            _fov = MathHelper.DegreesToRadians(angle);
        }
    }

    public Matrix4 ViewMatrix => Matrix4.LookAt(Position, Position + _front, _up);

    public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.1f, 100f);

    public void HandleKeyboardState(KeyboardState input, FrameEventArgs e)
    {
        if (input.IsKeyDown(Keys.W))
        {
            Position += _front * MovementSpeed * (float)e.Time; // Forward
        }

        if (input.IsKeyDown(Keys.S))
        {
            Position -= _front * MovementSpeed * (float)e.Time; // Backwards
        }

        if (input.IsKeyDown(Keys.A))
        {
            Position -= _right * MovementSpeed * (float)e.Time; // Left
        }

        if (input.IsKeyDown(Keys.D))
        {
            Position += _right * MovementSpeed * (float)e.Time; // Right
        }

        if (input.IsKeyDown(Keys.Space))
        {
            Position += _up * MovementSpeed * (float)e.Time; // Up
        }

        if (input.IsKeyDown(Keys.LeftShift))
        {
            Position -= _up * MovementSpeed * (float)e.Time; // Down
        }

        if (input.IsKeyDown(Keys.Up))
        {
            Pitch += RotationSpeed * (float)e.Time;
        }

        if (input.IsKeyDown(Keys.Down))
        {
            Pitch -= RotationSpeed * (float)e.Time;
        }

        if (input.IsKeyDown(Keys.Left))
        {
            Yaw -= RotationSpeed * (float)e.Time;
        }

        if (input.IsKeyDown(Keys.Right))
        {
            Yaw += RotationSpeed * (float)e.Time;
        }
    }

    private void UpdateLocalVectors()
    {
        // Calculate the front matrix
        _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
        _front.Y = MathF.Sin(_pitch);
        _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

        // Results need to be normalized
        _front = Vector3.Normalize(_front);

        // Calculate up and right vectors
        _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        _up = Vector3.Normalize(Vector3.Cross(_right, _front));
    }
}