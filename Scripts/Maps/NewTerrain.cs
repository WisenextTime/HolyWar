using System.Collections.Immutable;

using Godot;

namespace HolyWar.Maps;

public class NewTerrain
{
    public string Name { get; init; }
    public Mesh Mesh { get; init; }
    public virtual TerrainType Type { get; init; } = TerrainType.Land;
    public TileProperties Properties { get; init; }
}

public class NewTerrainFeature
{
    public string Name { get; init; }
    public Mesh Mesh { get; init; }

    public TileProperties Properties { get; init; }

    public int Priority { get; init; } = 0;
    public bool Overwriting { get; init; } = false;
    public ImmutableList<NewTerrain> OccursOn { get; init; } = [];
}