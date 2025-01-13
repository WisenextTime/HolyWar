using Godot;
using HolyWar.Scripts.Maps;

namespace HolyWar.Scripts.Game;

// ReSharper disable once InconsistentNaming
public partial class DebugGame_Map : Game
{
	private bool _mouseLeftButtonPressed = false;
	public override void _Ready()
	{
		var generator = new DefaultMapGenerator();
		var map = generator.Generate();
		GameMap.Map = map;
		GameMap.DrawMap();
		Camera.Position = new Vector3(map.Size * 1.732f, 10, 0);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		switch (@event)
		{
			case InputEventMouseButton mouseButton:
				_mouseLeftButtonPressed = mouseButton.Pressed;
				break;
			case InputEventMouseMotion mouseMotion:
				if (_mouseLeftButtonPressed)
				{
					Camera.Position += new Vector3(mouseMotion.Relative.X / 50, 0, mouseMotion.Relative.Y / 50);
				}
				break;
		}
	}
}