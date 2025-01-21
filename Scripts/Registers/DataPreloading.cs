using Godot;

using HolyWar.Core;
using HolyWar.Index;

using KirisameLib.Extensions;

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
        new Terrain[]
        {
            new("Void")
            {
                Color = new Color("#000000"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Void.tres"),
                Type = Terrain.TileType.Empty
            },
            new("Grassland")
            {
                Color = new Color("#0F2F1B"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Grass.tres"),
                Produces = { { "Food", 2 } }
            },
            new("Plain")
            {
                Color = new Color("#194328"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Plain.tres"),
                Produces = { { "Food", 1 }, { "Production", 1 } }
            },
            new("Tundra")
            {
                Color = new Color("#6B5C45"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Tundra.tres"),
                Produces = { { "Food", 1 } }
            },
            new("Snow")
            {
                Color = new Color("#D4D4D4"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Snow.tres"),
            },
            new("Desert")
            {
                Color = new Color("#E39E61"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Desert.tres"),
            },
            new("Lake")
            {
                Color = new Color("#9EECFF"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Lake.tres"),
                Type = Terrain.TileType.Water,
                PreventingFreshWater = true,
                Produces = { { "Food", 2 }, { "Gold", 1 } }
            },
            new WaterTerrain("Coast")
            {
                Color = new Color("#00ECFF"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Coast.tres"),
                Type = Terrain.TileType.Water,
                IsCoast = true,
                Produces = { { "Food", 1 }, { "Gold", 1 } }
            },
            new WaterTerrain("Ocean")
            {
                Color = new Color("#006BFF"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Ocean.tres"),
                Type = Terrain.TileType.Water,
                IsOcean = true,
                Produces = { { "Food", 1 }, { "Gold", 1 } }
            },
            new("Mountain")
            {
                Color = new Color("#A1A1A1"),
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Mount.tres"),
                IsImpassable = true,
                IsRoughArea = true,
                DefenseBonus = 0.25f
            },
            new LargeRiver("River")
            {
                Color = new Color("#00EEFF"),
                IsFreshWater = true,
                Produces = { { "Gold", 1 } },
                MovementCost = 0,
                DefenseBonus = -0.2f,
            }
        }.ForEach(terrain => DataRegisters.TerrainsRegister.RegisterItem(terrain.Name, terrain));

        /* Terrain features:
         * Hill, Forest, Jungle, Marsh, Oasis, FloodPlain, Ice, Atoll
         * Fallout, River
         */
        new TerrainFeature[]
        {
            new("Hill")
            {
                Color = default,
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Hill.tres"),
                Produces = { { "Production", 2 } },
                Overwriting = true,
                IsRoughArea = true,
                OccursOn = ["Tundra", "Plain", "Grassland", "Desert", "Snow"],
                MovementCost = 2,
                DefenseBonus = 0.25f,
                MaterialSameAs = true,
                EdgeRender = true
            },
            new("Forest")
            {
                Color = default,
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Tree.tres"),
                Produces = { { "Food", 1 }, { "Production", 1 } },
                IsRoughArea = true,
                Overwriting = true,
                OccursOn = ["Tundra", "Plain", "Grassland", "Hill"],
                MovementCost = 2,
                DefenseBonus = 0.25f
            },
            new("Ice")
            {
                Color = default,
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Ice.tres"),
                Overwriting = true,
                IsImpassable = true,
                OccursOn = ["Coast", "Ocean"],
            },
            new("Atoll")
            {
                Color = default,
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Atoll.tres"),
                Produces = { { "Food", 1 }, { "Production", 1 } },
                IsRare = true,
                OccursOn = ["Coast"],
            },
            new("Oasis")
            {
                Color = default,
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Oasis.tres"),
                Produces = { { "Food", 3 }, { "Gold", 1 } },
                OccursOn = ["Desert"],
                IsFreshWater = true,
                IsRare = true,
                IsUnbuildable = true,
                DefenseBonus = -0.1f
            },
            new("Marsh")
            {
                Color = default,
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Marsh.tres"),
                Produces = { { "Food", -1 } },
                DefenseBonus = -0.15f,
                IsUnbuildable = true,
                IsRare = true,
                MovementCost = 3,
                OccursOn = ["Grassland"]
            },
            new("FloodPlain")
            {
                Color = default,
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Flood.tres"),
                Produces = { { "Food", 2 } },
                DefenseBonus = -0.1f,
                IsRare = true,
                OnlyOnFreshWater = true,
                OccursOn = ["Desert"]
            },
            new("Jungle")
            {
                Color = default,
                Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Jungle.tres"),
                Produces = { { "Food", 2 } },
                IsRoughArea = true,
                Overwriting = true,
                OccursOn = ["Plain", "Grassland", "Hill"],
                MovementCost = 2,
                DefenseBonus = 0.25f
            },
        }.ForEach(feature => DataRegisters.TerrainFeaturesRegister.RegisterItem(feature.Name, feature));
    }
}