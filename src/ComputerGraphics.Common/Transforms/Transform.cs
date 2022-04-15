using OpenTK.Mathematics;

namespace ComputerGraphics.Common.Transforms;

public class Transform : ITransform
{
    private Matrix4 _modelMatrix;
    private Vector3 _position;
    private Quaternion _rotation;
    private Vector3 _scale;

    /// <summary>
    /// Current forward vector.
    /// </summary>
    /// <remarks>
    /// Default points to -Z.
    /// </remarks>
    private Vector3 _forward = -Constants.WorldForwardVector;

    /// <summary>
    /// Current up vector.
    /// </summary>
    /// <remarks>
    /// Default points to +Y.
    /// </remarks>
    private Vector3 _up = Constants.WorldUpVector;

    /// <summary>
    /// Current right vector.
    /// </summary>
    /// <remarks>
    /// Default points to +X.
    /// </remarks>
    private Vector3 _right = Constants.WorldRightVector;

    public Transform()
    : this(position: Vector3.Zero)
    {
    }

    public Transform(Vector3 position)
    : this(position, rotation: new Quaternion(Vector3.Zero))
    {
    }

    public Transform(Vector3 position, Quaternion rotation)
    : this(position, rotation, scale: Vector3.One)
    {
    }

    public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        _modelMatrix = Matrix4.Identity;

        Position = position;
        Scale = scale;
        Rotation = rotation;
    }

    /// <summary>
    /// Gets the model matrix of this transform computed using position, rotation, and scale.
    /// </summary>
    public Matrix4 ModelMatrix => _modelMatrix;

    /// <summary>
    /// Gets or sets the position of the entity.
    /// </summary>
    public Vector3 Position
    {
        get { return _position; }
        set
        {
            _position = value;
            UpdateModelMatrixPosition();
        }
    }

    /// <summary>
    /// Gets or sets the scale of the entity.
    /// </summary>
    public Vector3 Scale
    {
        get { return _scale; }
        set
        {
            _scale = value;
            UpdateModelMatrixScale();
        }
    }

    /// <summary>
    /// Gets or sets the rotation of the entity.
    /// </summary>
    public Quaternion Rotation
    {
        get { return _rotation; }
        set
        {
            _rotation = value;
            UpdateModelMatrixRotation();
        }
    }

    public void Translate(float x, float y, float z, bool world)
    {
        Translate(new Vector3(x, y, z), world);
    }

    public void Translate(Vector3 translation, bool world)
    {
        if (world)
        {
            _position += translation;
        }
        else
        {
            _position += _forward * translation.Z;
            _position += _up * translation.Y;
            _position += _right * translation.X;
        }

        UpdateModelMatrixPosition();
    }

    public void MoveForward(float delta, bool world)
    {
        _position += (world ? Constants.WorldForwardVector : _forward) * delta;
        UpdateModelMatrixPosition();
    }

    public void MoveUp(float delta, bool world)
    {
        _position += (world ? Constants.WorldUpVector : _up) * delta;
        UpdateModelMatrixPosition();
    }

    public void MoveRight(float delta, bool world)
    {
        _position += (world ? Constants.WorldRightVector : _right) * delta;
        UpdateModelMatrixPosition();
    }

    public void Rotate(float x, float y, float z, bool world)
    {
        Rotate(new Vector3(x, y, z), world);
    }

    public void Rotate(Vector3 degrees, bool world)
    {
        Vector3 radians = new(
            x: MathHelper.DegreesToRadians(degrees.X),
            y: MathHelper.DegreesToRadians(degrees.Y),
            z: MathHelper.DegreesToRadians(degrees.Z));
        Quaternion rotationOnRadians = new(radians);

        _rotation = world
            ? rotationOnRadians * _rotation
            : _rotation * rotationOnRadians;

        UpdateModelMatrixRotation();
    }

    public Vector3 GetEulerAnglesInDegrees()
    {
        Vector3 radians = _rotation.ToEulerAngles();
        return new Vector3(
            x: MathHelper.RadiansToDegrees(radians.X),
            y: MathHelper.RadiansToDegrees(radians.Y),
            z: MathHelper.RadiansToDegrees(radians.Z));
    }

    public override string ToString()
    {
        string position = $"({_position.X}, {_position.Y}, {_position.Z})";

        Vector3 degrees = GetEulerAnglesInDegrees();
        string rotation = $"({degrees.X}, {degrees.Y}, {degrees.Z})";

        string scale = $"({_scale.X}, {_scale.Y}, {_scale.Z})";

        return $"Position: {position} | Rotation: {rotation} | Scale: {scale}";
    }

    private void UpdateModelMatrixPosition()
    {
        _modelMatrix.M41 = _position.X;
        _modelMatrix.M42 = _position.Y;
        _modelMatrix.M43 = _position.Z;
    }

    private void UpdateModelMatrixScale()
    {
        _modelMatrix.Column0 *= _scale.X;
        _modelMatrix.Column1 *= _scale.Y;
        _modelMatrix.Column2 *= _scale.Z;
    }

    private void UpdateModelMatrixRotation()
    {
        _modelMatrix = Matrix4.CreateFromQuaternion(_rotation);

        UpdateModelMatrixScale();
        UpdateModelMatrixPosition();
        UpdateLocalVectors();
    }

    private void UpdateLocalVectors()
    {
        _forward = (ModelMatrix * new Vector4(-Constants.WorldForwardVector, 0.0f)).Xyz;
        _up = (ModelMatrix * new Vector4(Constants.WorldUpVector, 0.0f)).Xyz;
        _right = Vector3.Cross(_forward, _up);
    }

    public static class Constants
    {
        public static readonly Vector3 WorldForwardVector = new(0.0f, 0.0f, 1.0f);
        public static readonly Vector3 WorldUpVector = new(0.0f, 1.0f, 0.0f);
        public static readonly Vector3 WorldRightVector = new(1.0f, 0.0f, 0.0f);
    }
}