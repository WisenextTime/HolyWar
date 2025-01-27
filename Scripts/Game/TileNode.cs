using Godot;

using HolyWar.Maps;

namespace HolyWar.Game;

public partial class TileNode : MeshInstance3D
{
    #region Getters

    private TileNode() { }

    private static PackedScene Scene => field ??= ResourceLoader.Load<PackedScene>("res://Scenes/Tile.tscn");

    public static TileNode Create() => Scene.Instantiate<TileNode>();

    public static TileNode Create(TileDisplayProperties displayProperties)
    {
        var result = Scene.Instantiate<TileNode>();
        result.Mesh = displayProperties.Mesh;
        if (!displayProperties.DrawEdge) result.Layers |= 0b_10;
        return result;
    }

    #endregion
}