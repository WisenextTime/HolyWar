using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

using HolyWar.Registers;

using KirisameLib.Extensions;

namespace HolyWar.Maps.Generation;

public class CommonMapBuilder : MapBuilder
{
    public CommonMapBuilder(Vector2I size, int seed = -1) : base(size, seed)
    {
        Map = new Map(size, DataRegisters.Terrains["Void"]);
        FastNoiseLite noise = new()
        {
            Seed = Seed, NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
        };
        noise.Frequency = 0.015f;
        _noiseImage = noise.GetSeamlessImage3D(size.X, size.Y, 100);
        noise.Frequency = 0.3f;
        _highFrequencyNoiseImage = noise.GetSeamlessImage3D(size.X, size.Y, 100);
        //todo: noise也许没必要烘焙成图像，因为可以直接取值
        //      这么说来，如果直接取值的话把块数和噪声坐标的比例固定一下就能解决缩放问题了，
        //      而且还能解决地图用的是六边形网格却要在方形像素里取数值的问题，我们直接取对应的矢量点的值就好了
    }

    #region Private Fields

    private Map Map { get; }
    private Random Random => field ??= new(Seed);
    private readonly IList<Image> _noiseImage;
    private readonly IList<Image> _highFrequencyNoiseImage;

    #endregion


    #region Generation

    public CommonMapBuilder InitLandWater(NewTerrain land, NewTerrain coast, NewTerrain ocean, NewTerrain lake,
                                          float seaLevel = -0.05f, float coastLevel = 0.02f)
    {
        HashSet<Vector2I> nativeCoasts = [];

        foreach (var (coord, tile) in Map.TilesWithCoord)
        {
            var noise = GetNoise(_noiseImage, coord, 0);
            tile.MainTerrain =
                noise > seaLevel ? land :
                noise > seaLevel - coastLevel ? coast : ocean;

            if (noise <= seaLevel - coastLevel) // for oceans, fix coastline
            {
                if (Map.GetNeighborsPos(coord).Any(c => GetNoise(_noiseImage, c, 0) > seaLevel)) // any land in neighbors
                    tile.MainTerrain = coast;
            }
            else if (noise <= seaLevel) nativeCoasts.Add(coord);
        }

        HashSet<Vector2I> traversed = [];
        Queue<Vector2I> searching = [];
        List<Vector2I> toChange = [];
        foreach (var coord in nativeCoasts)
        {
            if (!traversed.Add(coord)) continue;

            searching.Clear();
            searching.Enqueue(coord);
            toChange.Clear();
            toChange.Add(coord);

            bool pass = false;
            bool? isLand = null;
            while (searching.TryDequeue(out var current))
            {
                foreach (var neighbor in Map.GetNeighborsPos(current))
                {
                    if (traversed.Contains(neighbor)) continue;

                    if (nativeCoasts.Contains(neighbor))
                    {
                        searching.Enqueue(neighbor);
                        traversed.Add(neighbor);
                        if (!pass) toChange.Add(neighbor);
                    }
                    else if (!pass)
                    {
                        var noise = GetNoise(_noiseImage, neighbor, 0);
                        if (isLand is null) isLand = noise > seaLevel;
                        else if (isLand != noise > seaLevel) pass = true;
                    }
                }
            }

            if (pass || isLand is null) continue;
            toChange.Select(c => Map[c]).ForEach(t => t.MainTerrain = isLand.Value ? land : ocean);
        }

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