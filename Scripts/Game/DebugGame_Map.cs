using Godot;
using HolyWar.Scripts.Maps;

namespace HolyWar.Scripts.Game;

// ReSharper disable once InconsistentNaming
public partial class DebugGame_Map : Game
{
	private bool _mouseLeftButtonPressed;
	private Map Map => GameMap.Map;
	// ReSharper disable once PossibleLossOfFraction
	private float CameraRightEdge => (Map.Size * 3f + 2) * 1.73205f;
	// ReSharper disable once PossibleLossOfFraction
	private float CameraLeftEdge => Map.Size * 1.73205f;
	public override void _Ready()
	{
		var generator = new DefaultMapGenerator();
		var map = generator.Generate();
		GameMap.Map = map;
		GameMap.DrawMap();
		var cameraPos = GameMap.ToRenderCoord(new Vector2(Map.Size, Map.Size));
		Camera.Position = new Vector3(cameraPos.X, 10, cameraPos.Z);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		switch (@event)
		{
			case InputEventMouseButton mouseButton:
				switch (mouseButton.ButtonIndex)
				{
					case MouseButton.Left:
						_mouseLeftButtonPressed = mouseButton.Pressed;
						break;
					case MouseButton.WheelDown:
						Camera.Position = new Vector3(Camera.Position.X, float.Min(25, Camera.Position.Y + 0.5f),
							Camera.Position.Z);
						break;
					case MouseButton.WheelUp:
						Camera.Position = new Vector3(Camera.Position.X, float.Max(10, Camera.Position.Y - 0.5f),
							Camera.Position.Z);
						break;
				}
				break;
			case InputEventMouseMotion mouseMotion:
				if (_mouseLeftButtonPressed)
				{
					Camera.Position += new Vector3(mouseMotion.Relative.X / 50, 0, mouseMotion.Relative.Y / 50) *
						Camera.Position.Y / 15;
					if (Camera.Position.X > CameraRightEdge) 
						Camera.Position = new Vector3(CameraLeftEdge, Camera.Position.Y, Camera.Position.Z);
					if(Camera.Position.X < CameraLeftEdge)
						Camera.Position = new Vector3(CameraRightEdge, Camera.Position.Y, Camera.Position.Z);
					
				}
				break;
		}
	}
}