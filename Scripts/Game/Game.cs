using Godot;

namespace HolyWar.Scripts.Game;

public partial class Game : Node
{
	protected GameMap GameMap => field??= GetNode<GameMap>("Map");
	protected Camera3D Camera => field??= GetNode<Camera3D>("Camera");
}