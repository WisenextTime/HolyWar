using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

using HolyWar.Registers;

using KirisameLib.Extensions;

namespace HolyWar.Maps.Generation;

public class CommonMapBuilder(Vector2I size, int seed = -1) : MapBuilder(size, seed)
{
    #region Private Fields

    private Map Map { get; } = new Map(size, DataRegisters.Terrains["Void"]); //todo: 改掉这个默认地形
    private Random Random => field ??= new(Seed);
    private FastNoiseLite Noise => field ??= new() { Seed = Seed, NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin };
    private IList<Image> NoiseImage
    {
        get
        {
            if (field is not null) return field;
            Noise.Frequency = 0.015f;
            return field = Noise.GetSeamlessImage3D(Size.X, Size.Y, 100);
        }
    }
    private IList<Image> HighFrequencyNoiseImage
    {
        get
        {
            if (field is not null) return field;
            Noise.Frequency = 0.3f;
            return field = Noise.GetSeamlessImage3D(Size.X, Size.Y, 100);
        }
    }

    #endregion


    #region Generation

    public CommonMapBuilder InitLandWater(NewTerrain land, NewTerrain coast, NewTerrain ocean, NewTerrain lake,
                                          float seaLevel = 0, float coastLevel = 0.2f)
    {
        HashSet<Vector2I> nativeCoasts = [];

        foreach (var (coord, tile) in Map.TilesWithCoord)
        {
            var noise = GetNoise(NoiseImage, coord, 0);
            tile.MainTerrain =
                noise > seaLevel ? land :
                noise > seaLevel - coastLevel ? coast : ocean;

            if (noise <= seaLevel - coastLevel) // for oceans, fix coastline
            {
                if (Map.GetNeighborsPos(coord).Any(c => GetNoise(NoiseImage, c, 0) > seaLevel)) // any land in neighbors
                    tile.MainTerrain = coast;
            }
            else if (noise <= seaLevel) nativeCoasts.Add(coord);
        }

        // HashSet<Vector2I> traversed = [];
        // Queue<Vector2I> searching = [];
        // List<Vector2I> toChange = [];
        // foreach (var coord in nativeCoasts)
        // {
        //     if (!traversed.Add(coord)) continue;
        //
        //     searching.Clear();
        //     searching.Enqueue(coord);
        //     toChange.Clear();
        //     toChange.Add(coord);
        //
        //     bool pass = true;
        //     bool? isLand = null;
        //     while (searching.TryDequeue(out var current))
        //     {
        //         pass = false;
        //         isLand = null;
        //         foreach (var neighbor in Map.GetNeighborsPos(current))
        //         {
        //             if (!traversed.Add(neighbor)) continue;
        //
        //             if (nativeCoasts.Contains(neighbor))
        //             {
        //                 searching.Enqueue(neighbor);
        //                 if (!pass) toChange.Add(neighbor);
        //             }
        //             else if (!pass)
        //             {
        //                 var noise = GetNoise(NoiseImage, neighbor, 0);
        //                 if (isLand is null) isLand = noise > seaLevel;
        //                 else if (isLand != noise > seaLevel) pass = true;
        //             }
        //         }
        //     }
        //
        //     if (pass || isLand is null) continue;
        //     toChange.Select(c => Map[c]).ForEach(t => t.MainTerrain = isLand.Value ? land : ocean);
        // }

        return this;
    }

    public override Map GetMap() => Map;

    #endregion


    #region Private Methods

    private static float GetNoise(IList<Image> noise, Vector2I pos, int z) => GetNoise(noise, pos.X, pos.Y, z);

    private static float GetNoise(IList<Image> noise, int x, int y, int z)
    {
        var noiseValue = noise[z].GetPixel(x, y).R;
        return noiseValue * 2 - 1;
    }

    #endregion
}