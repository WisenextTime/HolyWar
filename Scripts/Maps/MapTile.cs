using System.Collections.Generic;
using System.Linq;
using Godot;
using HolyWar.Scripts.Core;

namespace HolyWar.Scripts.Maps;

public struct MapTile()
{
	public string MainTerrain = "Void";
	public List<string> Features = [];
	public List<string> Improvements = []; 
	public Dictionary<string, int> Produces = [];
	public float DefenseBonus = 0;
	public int MovementCost = 1;
	
	public bool IsFreshWater = false;
	public bool IsRough = false;
	public bool IsOpen = false;
	public bool PreventingFreshWater = false;

	public Terrain GetMainTerrain() => Globals.Global.Terrains[MainTerrain];
	public List<Terrain> GetFeatures() => Features.Select(f => Globals.Global.Terrains[f]).ToList();
	public Terrain.TileType GetTileType() => GetMainTerrain().Type;
	public bool IsWater() => GetTileType() == Terrain.TileType.Water;
	public bool IsLand() => GetTileType() == Terrain.TileType.Land;
	
	public static MapTile VoidTile = new()
	{
		MainTerrain = "Void",
	};

	public static MapTile FromTerrain(string terrain)
	{
		var source = Globals.Global.Terrains[terrain];
		var tile = new MapTile
		{
			MainTerrain = terrain,
			IsOpen = source.IsOpenArea,
			IsRough = source.IsRoughArea,
			IsFreshWater = source.IsFreshWater,
			Produces = source.Produces,
			DefenseBonus = source.DefenseBonus,
			PreventingFreshWater = source.PreventingFreshWater,
			MovementCost = source.MovementCost,
		};
		return tile;
	}

	public void Sync()
	{
		var source = Globals.Global.Terrains[MainTerrain];
		IsOpen = source.IsOpenArea;
		IsRough = source.IsRoughArea;
		IsFreshWater = source.IsFreshWater;
		Produces = source.Produces;
		DefenseBonus = source.DefenseBonus;
		PreventingFreshWater = source.PreventingFreshWater;
		MovementCost = source.MovementCost;

		foreach (var terrain in GetFeatures())
		{
			if (terrain is not TerrainFeature feature) continue;
			if (feature.OverwritingProduces)
			{
				IsOpen = feature.IsOpenArea;
				IsRough = feature.IsRoughArea;
				IsFreshWater = feature.IsFreshWater;
				Produces = feature.Produces;
				DefenseBonus = feature.DefenseBonus;
				PreventingFreshWater = feature.PreventingFreshWater;
				MovementCost = feature.MovementCost;
			}
			else
			{
				IsOpen |= feature.IsOpenArea;
				IsRough |= feature.IsRoughArea;
				IsFreshWater |= feature.IsFreshWater;
				Produces = Produces
					.Concat(feature.Produces)
					.GroupBy(kvp => kvp.Key)
					.ToDictionary(g => g.Key, g => g.Sum(kvp => kvp.Value));
				DefenseBonus += feature.DefenseBonus;
				PreventingFreshWater |= feature.PreventingFreshWater;
				if (feature.MovementCost == 0) MovementCost = feature.MovementCost;
			}
		}
	}
}