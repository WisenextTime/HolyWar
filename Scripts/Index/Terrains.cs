using System.Collections.Generic;
using Godot;
using HolyWar.Scripts.Core;

namespace HolyWar.Scripts.Index;

public static class Terrains
{
	/*
	 * Normal terrains:
	 * Void
	 * Grassland, Plain, Tundra, Snow, Desert, Lake, Coast, Ocean, Mountain
	 *
	 * Terrain features:
	 * Hill, Forest, Jungle, Marsh, Oasis, FloodPlain, Ice, Atoll
	 * Fallout, River
	 */
	public static Dictionary<string, Terrain> PreloadIndexTerrains()
	{
		var terrains = new Dictionary<string, Terrain>
		{
			{
				"Void", new Terrain("Void")
				{
					Color = new Color("#000000"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Void.tres"),
					Type = Terrain.TileType.Empty
				}
			},
			{
				"Grassland", new Terrain("Grassland")
				{
					Color = new Color("#0F2F1B"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Grass.tres"),
					Produces = { { "Food", 2 } }
				}
			},
			{
				"Plain", new Terrain("Plain")
				{
					Color = new Color("#194328"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Plain.tres"),
					Produces = { { "Food", 1 }, { "Production", 1 } }
				}
			},
			{
				"Tundra", new Terrain("Tundra")
				{
					Color = new Color("#6B5C45"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Tundra.tres"),
					Produces = { { "Food", 1 } }
				}
			},
			{
				"Snow", new Terrain("Snow")
				{
					Color = new Color("#D4D4D4"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Snow.tres"),
				}
			},
			{
				"Desert", new Terrain("Desert")
				{
					Color = new Color("#E39E61"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Desert.tres"),
				}
			},
			{
				"Lake", new Terrain("Lake")
				{
					Color = new Color("#9EECFF"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Lake.tres"),
					Type = Terrain.TileType.Water,
					PreventingFreshWater = true,
					Produces = { { "Food", 2 }, { "Gold", 1 } }
				}
			},
			{
				"Coast", new Terrain("Coast")
				{
					Color = new Color("#00ECFF"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Coast.tres"),
					Type = Terrain.TileType.Water,
					IsCoast = true,
					Produces = { { "Food", 1 }, { "Gold", 1 } }
				}
			},
			{
				"Ocean", new Terrain("Ocean")
				{
					Color = new Color("#006BFF"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Ocean.tres"),
					Type = Terrain.TileType.Water,
					IsOcean = true,
					Produces = { { "Food", 1 }, { "Gold", 1 } }
				}
			},
			{
				"Mountain", new Terrain("Mountain")
				{
					Color = new Color("#A1A1A1"),
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Mount.tres"),
					IsImpassable = true,
					IsRoughArea = true,
					DefenseBonus = 0.25f
				}
			},
			{
				"Hill", new TerrainFeature("Hill")
				{
					Color = default,
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Hill.tres"),
					Produces = { { "Production", 2 } },
					OverwritingProduces = true,
					IsRoughArea = true,
					OccursOn = ["Tundra", "Plain", "Grassland", "Desert", "Snow"],
					MovementCost = 2,
					DefenseBonus = 0.25f,
					MaterialSameAs = true,
					EdgeRender = true

				}
			},
			{
				"Forest", new TerrainFeature("Forest")
				{
					Color = default,
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Tree.tres"),
					Produces = { { "Food", 1 }, { "Production", 1 } },
					IsRoughArea = true,
					OverwritingProduces = true,
					OccursOn = ["Tundra", "Plain", "Grassland", "Hill"],
					MovementCost = 2,
					DefenseBonus = 0.25f
				}
			},
			{
				"Ice", new TerrainFeature("Ice")
				{
					Color = default,
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Ice.tres"),
					OverwritingProduces = true,
					IsImpassable = true,
					OccursOn = ["Coast", "Ocean"],
				}
			},
			{
				"Atoll", new TerrainFeature("Atoll")
				{
					Color = default,
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Atoll.tres"),
					Produces = { { "Food", 1 }, { "Production", 1 } },
					IsRare = true,
					OccursOn = ["Coast"],
				}
			},
			{
				"Oasis", new TerrainFeature("Oasis")
				{
					Color = default,
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Oasis.tres"),
					Produces = { { "Food", 3 }, { "Gold", 1 } },
					OccursOn = ["Desert"],
					IsFreshWater = true,
					IsRare = true,
					IsUnbuildable = true,
					DefenseBonus = -0.1f
				}
			},
			{
				"Marsh", new TerrainFeature("Marsh")
				{
					Color = default,
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Marsh.tres"),
					Produces = { { "Food", -1 } },
					DefenseBonus = -0.15f,
					IsUnbuildable = true,
					IsRare = true,
					MovementCost = 3,
					OccursOn = ["Grassland"]
				}
			},
			{
				"FloodPlain", new TerrainFeature("FloodPlain")
				{
					Color = default,
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Flood.tres"),
					Produces = { { "Food", 2 } },
					DefenseBonus = -0.1f,
					IsRare = true,
					OnlyOnFreshWater = true,
					OccursOn = ["Desert"]
				}
			},
			{
				"Jungle", new TerrainFeature("Jungle")
				{
					Color = default,
					Mesh = ResourceLoader.Load<Mesh>("res://Assets/Models/Tiles/Jungle.tres"),
					Produces = { { "Food", 2 } },
					IsRoughArea = true,
					OverwritingProduces = true,
					OccursOn = ["Plain", "Grassland", "Hill"],
					MovementCost = 2,
					DefenseBonus = 0.25f
				}
			},
			{
				"River", new LargeRiver("River")
				{
					Color = new Color("#00EEFF"),
					IsFreshWater = true,
					Produces = { { "Gold", 1 } },
					MovementCost = 0,
					DefenseBonus = -0.2f,
				}
			}
		};


		return terrains;
	}
}