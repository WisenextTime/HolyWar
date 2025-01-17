using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Godot;
using Godot.Collections;
using HolyWar.Scripts.Core;
using Array = Godot.Collections.Array;

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
public class DefaultMapGenerator(
	int size = 25,
	int seed = default,
	float elevation = 0.70f,
	float maxTemperature = 0.60f,
	float temperatureVariation = 0.0f,
	float vegetation = 0.4f,
	float rareFeature = 0.05f,
	float seaLevel = 0.0f) : MapGenerator(size, seed, elevation, maxTemperature, temperatureVariation, vegetation,
	rareFeature, seaLevel)
{
	private readonly float _seaLevel = seaLevel - 0.05f;
	private readonly int _size = size;
	private readonly float _maxTemperature = maxTemperature;
	private readonly float _temperatureVariation = temperatureVariation;
	private readonly float _elevation = elevation;
	private readonly float _vegetation = vegetation;
	
	private Array<Image> _noiseImage;
	private Array<Image> _highFrequencyNoiseImage;

	private void InitNoiseImage(FastNoiseLite noise)
	{
		noise.Frequency = 0.015f;
		_noiseImage = noise.GetSeamlessImage3D(2 * _size + 2, 2 * _size, 100);
		noise.Frequency = 0.3f;
		_highFrequencyNoiseImage = noise.GetSeamlessImage3D(2 * _size + 2, 2 * _size, 100);
	}

	private float GetNoise(int x, int y, int z)
	{
		var noiseValue = _noiseImage[z].GetPixel(x, y).R;
		return noiseValue * 2 - 1;
	}
	
	private float GetNoise(Vector2I pos, int z) => GetNoise(pos.X, pos.Y, z);

	private float GetHighFrequencyNoise(int x, int y, int z)
	{
		var noiseValue = _highFrequencyNoiseImage[z].GetPixel(x, y).R;
		return noiseValue * 2 - 1;
	}

	private Vector2I GetNextPoint(Random random)
	{
		var x = random.Next(0, _size);
		var y = random.Next(0, _size);
		return new Vector2I(x, y);
	}
	
	private float GetHighFrequencyNoise(Vector2I pos, int z) => GetHighFrequencyNoise(pos.X, pos.Y, z);

	public override Map Generate()
	{
		var noise = new FastNoiseLite { Seed = Seed, NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin};
		InitNoiseImage(noise);
		var map = new Map
		{
			Size = _size,
			Seed = Seed,
		};
		var rand = new Random(Seed);
		map.InitTiles();
		//Init land and ocean
		foreach (var kvp in map.Tiles)
		{
			var seaNoise = GetNoise(kvp.Key, 0);
			if (seaNoise < _seaLevel && seaNoise > _seaLevel - 0.02f)
				kvp.Value.MainTerrain = "Coast";
			else if (seaNoise <= _seaLevel - 0.02f)
				kvp.Value.MainTerrain = "Ocean";
			else
				kvp.Value.MainTerrain = "Grassland";
		}

		// Fix coastline and gen lakes
		var fixedTiles = new List<Vector2I>();
		foreach (var kvp in map.Tiles.Where(kvp => !fixedTiles.Contains(kvp.Key)))
		{
			fixedTiles.Add(kvp.Key);
			switch (kvp.Value.MainTerrain)
			{
				case "Ocean":
				{
					if (map.GetNeighbors(kvp.Key).Any(t => t.IsLand())) kvp.Value.MainTerrain = "Coast";
					break;
				}
				case "Coast":
				{
					var searchList = new List<Vector2I> { kvp.Key };
					var changeList = new List<Vector2I> { kvp.Key };
					var edgeTiles = new List<Vector2I>();
					while (searchList.Count > 0)
					{
						var nowSearch = searchList[0];
						foreach (var neighbor in map.GetNeighbors(nowSearch))
						{
							var pos = map.GetTileCoord(neighbor);
							if (!changeList.Contains(pos) && neighbor.MainTerrain == "Coast")
							{
								changeList.Add(pos);
								searchList.Add(pos);
							}
							else if (neighbor.MainTerrain is "Ocean" or "Grassland" && !edgeTiles.Contains(pos)) 
								edgeTiles.Add(pos);
							
							searchList.Remove(nowSearch);
						}
					}
					fixedTiles.AddRange(changeList);
					if (edgeTiles.All(t => map.GetTile(t).MainTerrain == "Grassland"))
						foreach (var changeTile in changeList)
							map.GetTile(changeTile).MainTerrain = "Lake";
					if (edgeTiles.All(t => map.GetTile(t).MainTerrain == "Ocean"))
						foreach (var changeTile in changeList)
							map.GetTile(changeTile).MainTerrain = "Ocean";

					break;
				}
			}
		}
		
		// Gen more terrains
		foreach (var kvp in map.Tiles)
		{
			var temperature =
				float.Clamp(
					_maxTemperature -
					(_temperatureVariation + 1f / (_size * 0.8f)) * float.Abs(kvp.Key.Y - _size) +
					GetNoise(kvp.Key, 2) / 2,
					-1, 1);
			var humidity = GetNoise(kvp.Key, 5);

			var height = GetHighFrequencyNoise(kvp.Key, 3) / _elevation - (1 - _elevation);
			
			if (kvp.Value.MainTerrain == "Grassland")
			{
				kvp.Value.MainTerrain = (temperature, humidity) switch
				{
					(< -0.25f, _) => "Snow",
					(< -0.2f, > 0) => "Snow",
					(< -0f, _) => "Tundra",
					(< 0.4f, < 0) => "Plain",
					(>= 0.4f, < 0.3f) => "Desert",
					(_, _) => "Grassland"
				};
				//Hills and mountains
				switch (height)
				{
					case > 0.5f:
						kvp.Value.MainTerrain = "Mountain";
						break;
					case > 0f:
						kvp.Value.Features.Add("Hill");
						break;
				}
				//Gen vegetation
				var vegetation = (GetHighFrequencyNoise(kvp.Key, 5) + 1) / 2;
				if(kvp.Value.MainTerrain == "Mountain" || vegetation >= _vegetation) continue;
				switch (temperature)
				{
					case > 0.3f when kvp.Value.MainTerrain is "Grassland" or "Plain":
						kvp.Value.Features.Add("Jungle");
						break;
					case >-1 when kvp.Value.MainTerrain is not "Desert" and not "Snow":
						kvp.Value.Features.Add("Forest");
						break;
				}
				
			}
			else if (kvp.Value.IsWater() && temperature < -0.7f)
				kvp.Value.Features.Add("Ice");
		}
		
		//Gen rivers
		//TODO
		var riverCount = rand.Next(0, _size);
		for (var i = 0; i < riverCount; i++)
		{
			var nextRiverId = i;
			var riverTiles = new List<Vector2I>
			{
				GetNextPoint(rand)
			};
			while (true)
			{
				var nextPoint = riverTiles[^1];
				if (map.GetTile(nextPoint).IsWater())
					break;
				if (map.GetTile(nextPoint).GetRiverId() != -1)
				{
					nextRiverId = map.GetTile(nextPoint).GetRiverId();
				}
				MapTile connectPoint;
				do
				{
					var points = map.GetNeighbors(nextPoint);
					connectPoint = points[rand.Next(0, points.Count)];
				} while (riverTiles.Contains(map.GetTileCoord(connectPoint)) && connectPoint != MapTile.VoidTile);
				riverTiles.Add(map.GetTileCoord(connectPoint));
			}

			if (riverTiles.Count <= 1) continue;
			foreach (var tile in riverTiles)
			{
				map.GetTile(tile).LargeRivers.Add(new LargeRiverTile
				{
					RiverSource = "River",
					RiverId = nextRiverId
				});
				map.GetTile(tile).Features.Add("River");
			}
		}
		
		//The first sync
		foreach (var kvp in map.Tiles)
		{
			kvp.Value.Sync();
		}
		
		//Rare feature
		foreach (var rareFeature in Globals.Global.Terrains.Where(pair => pair.Value is TerrainFeature { IsRare: true }))
		{
			var feature = (TerrainFeature)rareFeature.Value;
			foreach (var occursTile in map.Tiles.Where(t => feature.OccursOn.Contains(t.Value.MainTerrain)))
			{
				if (rand.NextSingle() > feature.RareRate || occursTile.Value.Features.Count != 0) continue;
				occursTile.Value.Features.Add(feature.Name);
			}
		}

		return map;
	}
}

