using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Godot;
using Godot.Collections;
using HolyWar.Scripts.Core;

namespace HolyWar.Scripts.Maps;

public abstract class MapGenerator(int Size = 25,int seed = default,
	float Elevation = 0.70f,
	float MaxTemperature = 0.60f, float TemperatureVariation = 0.0f,
	float Vegetation = 0.4f, float RareFeature = 0.05f,
	float SeaLevel = 0.0f)
{
	public int Seed = seed==default ? (int)Time.GetTicksMsec() : seed;

	public void RandomizeSeed()
	{
		Seed = (int)Time.GetTicksMsec();
	}
	public abstract Map Generate();
}
// Based on Perlin Noise
public class DefaultMapGenerator(int size = 55,int seed = default,
	float elevation = 0.70f,
	float maxTemperature = 0.60f, float temperatureVariation = 0.0f,
	float vegetation = 0.4f, float rareFeature = 0.05f,
	float seaLevel = 0.0f) : MapGenerator(size, seed, elevation, maxTemperature, temperatureVariation, vegetation, rareFeature, seaLevel)
{
	private readonly float _seaLevel = seaLevel - 0.05f;
	private readonly int _size = size;

	public override Map Generate()
	{
		var noise = new FastNoiseLite
		{
			Seed = Seed,
			NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
			Frequency = 0.015f
		};
		var map = new Map
		{
			Size = _size,
			Seed = Seed,
		};
		map.InitTiles();
		//Init land and ocean
		foreach (var column in map.Tiles.Index())
		{
			foreach (var tile in column.Item.Index())
			{
				var seaNoise = noise.GetNoise2D(column.Index, tile.Index);
				if (seaNoise < _seaLevel && seaNoise > _seaLevel - 0.02f)
					tile.Item.MainTerrain = "Coast";
				else if (seaNoise <= _seaLevel - 0.02f)
					tile.Item.MainTerrain = "Ocean";
				else
					tile.Item.MainTerrain = "Grassland";
			}
		}
		//Fix coast line and gen lakes
		var fixedTiles = new List<Vector2I>();
		foreach (var column in map.Tiles.Index())
		{
			foreach (var tile in column.Item.Index())
			{
				var coord = map.TileToWorld(new Vector2I(column.Index, tile.Index));
				if(fixedTiles.Contains(coord)) continue;
				fixedTiles.Add(coord);
				switch (tile.Item.MainTerrain)
				{
					case "Ocean" when
						map.GetNeighbors(coord.X, coord.Y).Any(t => t.IsLand()):
						tile.Item.MainTerrain = "Coast";
						continue;
					case "Coast":
					{
						var searchList = new List<Vector2I> { coord };
						var changeList = new List<MapTile> { tile.Item };
						var edgeTiles = new List<MapTile>();
						while (searchList.Count != 0)
						{
						
							var nowCoord = searchList[0];
							foreach (var neighbor in map.GetNeighbors(nowCoord.X, nowCoord.Y))
							{
								if (neighbor.MainTerrain == "Coast" &&
								    !searchList.Contains(map.GetTileCoord(neighbor)) &&
								    !fixedTiles.Contains(map.GetTileCoord(neighbor)))
								{
									searchList.Add(map.GetTileCoord(neighbor));
									fixedTiles.Add(map.GetTileCoord(neighbor));
									changeList.Add(neighbor);
								}
								else if (neighbor.MainTerrain != "Coast" && neighbor.MainTerrain != "Void" && !edgeTiles.Contains(neighbor))
									edgeTiles.Add(neighbor);
							}
							searchList.Remove(nowCoord);
						}

						if (edgeTiles.All(t => t.IsLand()))
						{
							foreach (var mapTile in changeList)
							{
								mapTile.MainTerrain = "Lake";
							}
						}
						else if (edgeTiles.All(t => t.MainTerrain == "Ocean"))
						{
							foreach (var mapTile in changeList)
							{
								mapTile.MainTerrain = "Ocean";
							}
						}

						break;
					}
				}
			}
		}
		return map;
		
	}
}

