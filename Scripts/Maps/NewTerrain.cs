using System.Collections.Immutable;

using Godot;

namespace HolyWar.Maps;

public class NewTerrain(string name)
{
    public string Name => name;
    public TileDisplayProperties DisplayProperties { get; init; }

    public virtual TerrainType Type { get; init; } = TerrainType.Land;

    public TileProperties TileProperties { get; init; }
    public TerrainProperties TerrainProperties { get; init; }

    public static NewTerrain Default { get; } = new("Null")
    {
        DisplayProperties = new()
        {
            Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Void.tres"),
        },
        Type = TerrainType.Empty,
    };
}

public class NewTerrainFeature(string name)
{
    public string Name => name;
    public TileDisplayProperties DisplayProperties { get; init; }

    public TileProperties TileProperties { get; init; }
    public TerrainProperties TerrainProperties { get; init; }
    public bool SameTexture { get; init; }

    public bool IsRare { get; init; } = false;
    public int Priority { get; init; } = 0;
    public bool Overwriting { get; init; } = false;
    public bool OnlyOnFreshWater { get; init; } = false;
    public ImmutableList<NewTerrain> OccursOn { get; init; } = [];

    public static NewTerrainFeature Default { get; } = new("Null")
    {
        DisplayProperties = new()
        {
            Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Void.tres"),
        },
    };
}