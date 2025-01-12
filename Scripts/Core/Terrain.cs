using System.Collections.Generic;
using Godot;

namespace HolyWar.Scripts.Core;

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
	public bool IsRare = false;
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
}

public record TerrainFeature : Terrain
{
	public TerrainFeature(string Name) : base(Name)
	{
		Type = TileType.Empty;
	}
	public bool OverwritingProduces = false;
	public List<string> OccursOn = [];
}

public record River : TerrainFeature
{ 
	public River(string Name) : base(Name) { }
}