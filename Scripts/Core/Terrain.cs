﻿using System.Collections.Generic;

using Godot;

namespace HolyWar.Core;

public record Terrain(string Name)
{
	public string Name = Name;
	public Mesh Mesh;
	public TileType Type = TileType.Land;
	public int MovementCost = 1;
	public float DefenseBonus = 0;
	
	public bool IsImpassable = false;
	public bool IsOpenArea = true;
	public bool IsRoughArea = false;
	public bool PreventingFreshWater = false;
	public bool IsFreshWater = false;
	public bool IsUnbuildable = false;
	
	//only effort when this terrain is 'Water'
	public bool IsCoast = false;
	public bool IsOcean = false;
	
	public enum TileType
	{
		Empty,Land,Water
	}
	public Color Color;

	public Dictionary<string, int> Produces = [];
	
	public List<Unique> Uniques = [];
	public bool EdgeRender = true;
}

public record TerrainFeature : Terrain
{
	public float RareRate = 0.1f;
	public bool IsRare = false;
	public bool MaterialSameAs = false;
	public bool OnlyOnFreshWater = false;
	public TerrainFeature(string Name) : base(Name)
	{
		Type = TileType.Empty;
		EdgeRender = false;
	}
	public bool OverwritingProduces = false;
	public List<string> OccursOn = [];
}

// public record River : TerrainFeature
// {
// 	public River(string Name) : base(Name) { }
// }

public record LargeRiver : TerrainFeature
{
	public LargeRiver(string Name) : base(Name) { }
}