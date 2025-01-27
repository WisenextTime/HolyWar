using System;

using Godot;

namespace HolyWar.Game;

public partial class GameCamera : Camera3D
{
    #region Getters

    private GameCamera() { }

    private static PackedScene Scene => field ??= ResourceLoader.Load<PackedScene>("res://Scenes/GameCamera.tscn");

    public GameCamera Create() => (GameCamera)Scene.Instantiate();

    #endregion

    public float CameraLeftEdge { get; set; } = float.NegativeInfinity;
    public float CameraRightEdge { get; set; } = float.PositiveInfinity;
    public float CameraUpEdge { get; set; } = float.PositiveInfinity;
    public float CameraDownEdge { get; set; } = float.NegativeInfinity;
    public float CameraHighEdge { get; set; } = 25;
    public float CameraBottomEdge { get; set; } = 10;

    private bool _isDragging;

    public override void _Ready()
    {
        ClampPosition();
    }

    public void ClampPosition()
    {
        Position = new Vector3(Math.Clamp(Position.X, CameraLeftEdge,   CameraRightEdge),
                               Math.Clamp(Position.Y, CameraBottomEdge, CameraHighEdge),
                               Math.Clamp(Position.Z, CameraDownEdge,   CameraUpEdge));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        var handled = true;

        switch (@event)
        {
            case InputEventMouseButton mouseButton:
                switch (mouseButton.ButtonIndex)
                {
                    case MouseButton.Left:
                        _isDragging = mouseButton.Pressed;
                        break;
                    case MouseButton.WheelDown:
                        Position = new Vector3(Position.X, float.Min(25, Position.Y + 0.5f), Position.Z);
                        break;
                    case MouseButton.WheelUp:
                        Position = new Vector3(Position.X, float.Max(10, Position.Y - 0.5f), Position.Z);
                        break;
                }
                break;
            case InputEventMouseMotion mouseMotion:
                if (_isDragging)
                {
                    var position = Position + new Vector3(mouseMotion.Relative.X, 0, mouseMotion.Relative.Y) * Position.Y / 750;
                    if (position.X > CameraRightEdge)
                        position.X = CameraLeftEdge + position.X - CameraRightEdge;
                    else if (Position.X < CameraLeftEdge)
                        position.X = CameraRightEdge + position.X - CameraLeftEdge;
                    position.Z = Math.Clamp(position.Z, CameraDownEdge, CameraUpEdge);
                    Position = position;
                }
                break;
            default:
                handled = false;
                break;
        }

        if (handled) GetViewport().SetInputAsHandled();
    }
}