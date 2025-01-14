using Godot;

namespace HolyWar.Scripts.Game;

public partial class Game : Node
{
	protected GameMap GameMap => field??= GetNode<GameMap>("Map");
	[Export]
	protected Camera3D Camera { get; set; }
}