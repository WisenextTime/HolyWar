using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;

using Godot;

namespace HolyWar.Core;

public record Terrain(string Name)
{
    public Mesh Mesh { get; init; }
    public virtual TileType Type { get; init; } = TileType.Land;
    public int MovementCost { get; init; } = 1;
    public float DefenseBonus { get; init; } = 0;

    public bool IsImpassable { get; init; } = false;
    public bool IsOpenArea { get; init; } = true;
    public bool IsRoughArea { get; init; } = false;
    public bool PreventingFreshWater { get; init; } = false;
    public bool IsFreshWater { get; init; } = false;
    public bool IsUnbuildable { get; init; } = false;

    public Color Color { get; init; }

    public ImmutableDictionary<string, int> Produces { get; init; } = ImmutableDictionary.Create<string, int>();

    public ImmutableList<Unique> Uniques { get; init; } = [];
    public virtual bool EdgeRender { get; init; } = true;

    public static Terrain Default { get; } = new("Null")
    {
        Color = new Color("#000000"),
        Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Void.tres"),
        Type = TileType.Empty
    };

    public enum TileType
    {
        Empty, Land, Water
    }
}

public record WaterTerrain(string Name) : Terrain(Name)
{
    public override TileType Type => TileType.Water;

    public bool IsCoast { get; init; } = false;
    public bool IsOcean { get; init; } = false;
}

public record TerrainFeature(string Name) : Terrain(Name)
{
    public override TileType Type => TileType.Empty;
    public override bool EdgeRender => false;

    public float RareRate { get; init; } = 0.1f;
    public bool IsRare { get; init; } = false;
    public bool MaterialSameAs { get; init; } = false;
    public bool OnlyOnFreshWater { get; init; } = false;

    public int Priority { get; init; } = 0;
    public bool Overwriting { get; init; } = false;
    public ImmutableList<string> OccursOn { get; init; } = [];

    public new static TerrainFeature Default { get; } = new("Null")
    {
        Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Void.tres"),
    };
}

public record LargeRiver(string Name) : TerrainFeature(Name) { }