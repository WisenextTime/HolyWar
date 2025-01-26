using Godot;

namespace HolyWar.Game;

public partial class Game : Node
{
	protected OldGameMap GameMap => field??= GetNode<OldGameMap>("Map");
	[Export]
	protected Camera3D Camera { get; set; }
}