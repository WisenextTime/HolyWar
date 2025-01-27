using System.Collections.Immutable;
using System.Linq;

using Godot;
using Godot.Collections;

using HolyWar.Maps;

using KirisameLib.Extensions;

using Dict = System.Collections.Generic.Dictionary<string, int>;

namespace HolyWar.Registers;

public static class DataPreloading
{
    public static void PreloadTerrains()
    {
        /*
         * Normal terrains:
         * Void
         * Grassland, Plain, Tundra, Snow, Desert, Lake, Coast, Ocean, Mountain
         */
        new NewTerrain[]
        {
            new("Void")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#000000"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Void.tres"),
                },
                Type = TerrainType.Empty,
            },
            new("Grassland")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#0F2F1B"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Grass.tres"),
                },
                TileProperties = new()
                {
                    Produces = new Dict { ["Food"] = 2 }.ToImmutableDictionary(),
                },
            },
            new("Plain")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#194328"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Plain.tres"),
                },
                TileProperties = new()
                {
                    Produces = new Dict { ["Food"] = 1, ["Production"] = 1 }.ToImmutableDictionary(),
                },
            },
            new("Tundra")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#6B5C45"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Tundra.tres"),
                },
                TileProperties = new()
                {
                    Produces = new Dict { ["Food"] = 1 }.ToImmutableDictionary(),
                },
            },
            new("Snow")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#D4D4D4"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Snow.tres"),
                },
            },
            new("Desert")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#E39E61"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Desert.tres"),
                },
            },
            new("Lake")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#9EECFF"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Lake.tres"),
                },
                Type = TerrainType.Water,
                TerrainProperties = new()
                {
                    CanPreventFreshWater = true,
                },
                TileProperties = new()
                {
                    Produces = new Dict { ["Food"] = 2, ["Gold"] = 1 }.ToImmutableDictionary(),
                },
            },
            new("Coast")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#00ECFF"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Coast.tres"),
                },
                Type = TerrainType.Water,
                TerrainProperties = new()
                {
                    IsCoast = true,
                },
                TileProperties = new()
                {
                    Produces = new Dict { ["Food"] = 1, ["Gold"] = 1 }.ToImmutableDictionary(),
                },
            },
            new("Ocean")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#006BFF"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Ocean.tres"),
                },
                Type = TerrainType.Water,
                TerrainProperties = new()
                {
                    IsOcean = true,
                },
                TileProperties = new()
                {
                    Produces = new Dict { ["Food"] = 1, ["Gold"] = 1 }.ToImmutableDictionary(),
                },
            },
            new("Mountain")
            {
                DisplayProperties = new()
                {
                    Color = new Color("#A1A1A1"),
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Mount.tres"),
                },
                TerrainProperties = new()
                {
                    IsImpassable = true,
                    IsRough = true,
                },
                TileProperties = new()
                {
                    DefenseBonus = 0.25f
                },
            },
        }.ForEach(terrain => DataRegisters.TerrainsRegister.RegisterItem(terrain.Name, terrain));

        /* Terrain features:
         * Hill, Forest, Jungle, Marsh, Oasis, FloodPlain, Ice, Atoll
         * Fallout, River
         */
        new NewTerrainFeature[]
        {
            new("Hill")
            {
                DisplayProperties = new()
                {
                    Color = default,
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Hill.tres"),
                },
                TerrainProperties = new()
                {
                    IsRough = true,
                },
                TileProperties = new()
                {
                    MovementCost = 2,
                    DefenseBonus = 0.25f,
                    Produces = new Dict { ["Production"] = 2 }.ToImmutableDictionary(),
                },
                Overwriting = true,
                OccursOn = new[] { "Tundra", "Plain", "Grassland", "Desert", "Snow" }
                          .Select(id => DataRegisters.Terrains[id]).ToImmutableList(),
                SameTexture = true,
            },
            new("Forest")
            {
                DisplayProperties = new()
                {
                    Color = default,
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Tree.tres"),
                    DrawEdge = false
                },
                TerrainProperties = new()
                {
                    IsRough = true,
                },
                TileProperties = new()
                {
                    MovementCost = 2,
                    DefenseBonus = 0.25f,
                    Produces = new Dict { ["Food"] = 1, ["Production"] = 1 }.ToImmutableDictionary(),
                },
                Overwriting = true,
                OccursOn = new[] { "Tundra", "Plain", "Grassland", "Hill" }
                          .Select(id => DataRegisters.Terrains[id]).ToImmutableList(),
            },
            new("Ice")
            {
                DisplayProperties = new()
                {
                    Color = default,
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Ice.tres"),
                    DrawEdge = false
                },
                TerrainProperties = new()
                {
                    IsImpassable = true,
                },
                Overwriting = true,
                OccursOn = new[] { "Coast", "Ocean" }.Select(id => DataRegisters.Terrains[id]).ToImmutableList(),
            },
            new("Atoll")
            {
                DisplayProperties = new()
                {
                    Color = default,
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Atoll.tres"),
                    DrawEdge = false
                },
                TileProperties = new()
                {
                    Produces = new Dict { ["Food"] = 1, ["Production"] = 1 }.ToImmutableDictionary(),
                },
                IsRare = true,
                OccursOn = new[] { "Coast" }.Select(id => DataRegisters.Terrains[id]).ToImmutableList(),
            },
            new("Oasis")
            {
                DisplayProperties = new()
                {
                    Color = default,
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Oasis.tres"),
                    DrawEdge = false,
                },
                TerrainProperties = new()
                {
                    IsFreshWater = true,
                    IsUnbuildable = true,
                },
                TileProperties = new()
                {
                    DefenseBonus = -0.1f,
                    Produces = new Dict { ["Food"] = 3, ["Gold"] = 1 }.ToImmutableDictionary(),
                },
                IsRare = true,
                OccursOn = new[] { "Desert" }.Select(id => DataRegisters.Terrains[id]).ToImmutableList(),
            },
            new("Marsh")
            {
                DisplayProperties = new()
                {
                    Color = default,
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Marsh.tres"),
                    DrawEdge = false,
                },
                TerrainProperties = new()
                {
                    IsUnbuildable = true,
                },
                TileProperties = new()
                {
                    DefenseBonus = -0.15f, MovementCost = 3,
                    Produces = new Dict { ["Food"] = -1 }.ToImmutableDictionary(),
                },
                IsRare = true,
                OccursOn = new[] { "Grassland" }.Select(id => DataRegisters.Terrains[id]).ToImmutableList(),
            },
            new("FloodPlain")
            {
                DisplayProperties = new()
                {
                    Color = default,
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Flood.tres"),
                    DrawEdge = false
                },
                TileProperties = new()
                {
                    DefenseBonus = -0.1f,
                    Produces = new Dict { ["Food"] = 2 }.ToImmutableDictionary(),
                },
                IsRare = true,
                OccursOn = new[] { "Desert" }.Select(id => DataRegisters.Terrains[id]).ToImmutableList(),
                OnlyOnFreshWater = true,
            },
            new("Jungle")
            {
                DisplayProperties = new()
                {
                    Color = default,
                    Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Jungle.tres"),
                    DrawEdge = false
                },
                TerrainProperties = new()
                {
                    IsRough = true,
                },
                TileProperties = new()
                {
                    MovementCost = 2,
                    DefenseBonus = 0.25f,
                    Produces = new Dict { ["Food"] = 2 }.ToImmutableDictionary(),
                },
                Overwriting = true,
                OccursOn = new[] { "Plain", "Grassland", "Hill" }.Select(id => DataRegisters.Terrains[id]).ToImmutableList(),
            },
        }.ForEach(feature => DataRegisters.TerrainFeaturesRegister.RegisterItem(feature.Name, feature));
    }
}