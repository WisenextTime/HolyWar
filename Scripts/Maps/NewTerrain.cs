using System.Collections.Immutable;

using Godot;

namespace HolyWar.Maps;

public class NewTerrain
{
    public string Name { get; init; }
    public TileDisplayProperties DisplayProperties { get; init; }

    public virtual TerrainType Type { get; init; } = TerrainType.Land;

    public TileProperties TileProperties { get; init; }
    public TerrainProperties TerrainProperties { get; init; }
}

public class NewTerrainFeature
{
    public string Name { get; init; }
    public TileDisplayProperties DisplayProperties { get; init; }

    public TileProperties TileProperties { get; init; }
    public TerrainProperties TerrainProperties { get; init; }
    public bool SameTexture { get; init; }

    public int Priority { get; init; } = 0;
    public bool Overwriting { get; init; } = false;
    public ImmutableList<NewTerrain> OccursOn { get; init; } = [];
}