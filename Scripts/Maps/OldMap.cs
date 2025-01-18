using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

namespace HolyWar.Maps;

[Obsolete("Use new Map instead")]
public class OldMap(int size = 10)
{
	public string Name = string.Empty;
	public string Author = string.Empty;
	public string Description = string.Empty;
	public int Size = size;
	public int Seed = 0;
	public Dictionary<Vector2I,OldMapTile> Tiles = [];

	// public Vector2I TileToWorld(Vector2I tilePos) => new(tilePos.X - Size + 1, tilePos.Y);
	// public Vector2I WorldToTile(Vector2I worldPos) => new(worldPos.X + Size - 1, worldPos.Y);

	// public Vector2I GetTileCoord(MapTile target)
	// {
	// 	foreach (var column in Tiles.Index())
	// 	{
	// 		foreach (var tile in column.Item.Index())
	// 		{
	// 			if (tile.Item == target) return new Vector2I(column.Index, tile.Index);
	// 		}
	// 	}
	// 	return new Vector2I(-1, -1);
	// }

	// public MapTile GetTile(int x, int y)
	// {
	// 	var pos = WorldToTile(new Vector2I(x, y));
	// 	if (x <= -Size || x >= Size || y >= Tiles[pos.X].Count || y < 0)
	// 		return MapTile.VoidTile;
	// 	return Tiles[pos.X][pos.Y];
	// }

	public void InitTiles(string tileName = "Grassland")
	{
		for (var x = 0; x <= 2 * Size + 1; x++)
		{
			// var tiles = new List<MapTile>();
			// tiles.AddRange(
			// 	Enumerable.Repeat(new MapTile(), 2 * Size - 1 - (x < 0 ? -x : x))
			// 		.Select(t => MapTile.FromTerrain(tileName)));
			// Tiles.Add(tiles);
			for (var y = 0; y < 2 * Size; y++)
			{
				var tiles = OldMapTile.FromTerrain(tileName);
				Tiles[new Vector2I(x, y)] = tiles;
			}
		}
	}

	public List<OldMapTile> GetNeighbors(int x, int y)
	{
		var neighbors = new List<OldMapTile>
		{
			GetTile(x, y - 1),
			GetTile(x, y + 1)
		};
		if (y % 2 == 0)
		{
			neighbors.Add(GetTile(x - 1, y));
			neighbors.Add(GetTile(x - 1, y - 1));
			neighbors.Add(GetTile(x + 1, y));
			neighbors.Add(GetTile(x - 1, y + 1));
			
			if (x == Size + 1)
			{				
				neighbors.Add(GetTile(Size + 1, y));
			}

			if (x == 0)
			{
				neighbors.Add(GetTile(0, y));
				neighbors.Add(GetTile(0, y - 1));
				neighbors.Add(GetTile(0, y + 1));
			}
		}
		else
		{
			neighbors.Add(GetTile(x - 1, y));
			neighbors.Add(GetTile(x + 1, y + 1));
			neighbors.Add(GetTile(x + 1, y));
			neighbors.Add(GetTile(x + 1, y - 1));
			
			if (x == 0)
			{			
				neighbors.Add(GetTile(0, y));
			}

			if (x == Size + 1)
			{
				neighbors.Add(GetTile(Size + 1, y));
				neighbors.Add(GetTile(Size + 1, y - 1));
				neighbors.Add(GetTile(Size + 1, y + 1));
			}
		}
		return neighbors; 
	}
	
	public List<OldMapTile> GetNeighbors(Vector2I pos) => GetNeighbors(pos.X, pos.Y);

	public OldMapTile GetTile(int x, int y) => Tiles.GetValueOrDefault(new Vector2I(x, y), OldMapTile.VoidTile);
	public OldMapTile GetTile(Vector2I pos) => Tiles.GetValueOrDefault(pos, OldMapTile.VoidTile);

	public Vector2I GetTileCoord(OldMapTile neighbor)
	{
		foreach (var tile in Tiles.Where(tile => tile.Value == neighbor))
		{
			return tile.Key;
		}

		return new Vector2I(-1, -1);
	}
}