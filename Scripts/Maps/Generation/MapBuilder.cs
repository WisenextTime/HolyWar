using Godot;

namespace HolyWar.Maps.Generation;

/// <summary>
///
/// </summary>
/// <param name="size">Size of the map</param>
/// <param name="seed">Random seed that will be used for map generation, use -1 for a random seed</param>
public abstract class MapBuilder(int size, int seed = -1)
{
    public int Size => size;
    public int Seed { get; } = seed is -1 ? (int)Time.GetTicksMsec() : seed;

    public abstract OldMap GetMap();
}