using System;
using System.Collections.Generic;
using System.Linq;
using HolyWar.Scripts.Core;

namespace HolyWar.Scripts.Maps;

public class Map(int size = 10)
{
	public string Name = string.Empty;
	public string Author = string.Empty;
	public string Description = string.Empty;
	public int Size = size;
	public List<List<MapTile>> Tiles = [];

	public MapTile GetTile(int x, int y)
	{
		if (x <= -Size || x >= Size || y >= Tiles[x].Count)
			return MapTile.VoidTile;
		return Tiles[x][y];
	}

	public void InitTiles()
	{
		for (var x = -Size + 1; x <= Size; x++)
		{
			var tiles = new List<MapTile>();
			tiles.AddRange(
				Enumerable.Repeat(MapTile.FromTerrain("Grassland"), 2 * Size - 1 - (x < 0 ? -x : x)));
			Tiles.Add(tiles);
		}
	}
}