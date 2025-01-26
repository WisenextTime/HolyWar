using Godot;

namespace HolyWar.Maps;

public readonly record struct TileDisplayProperties
{
    public Mesh Mesh { get; init; }
    public bool DrawEdge { get; init; }
    public Color Color { get; init; }
}