using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Godot.Collections;

namespace HolyWar.Maps;

public readonly record struct TileProperties() : IAdditionOperators<TileProperties, TileProperties, TileProperties>
{
    public int MovementCost { get; init; }
    public float DefenseBonus { get; init; }
    public ImmutableDictionary<string, int> Produces { get; init; }

    public static TileProperties operator +(TileProperties a, TileProperties b) => new()
    {
        MovementCost = a.MovementCost + b.MovementCost,
        DefenseBonus = a.DefenseBonus + b.DefenseBonus,
        Produces = a.Produces.Concat(b.Produces)
                    .GroupBy(produce => produce.Key, produce => produce.Value)
                    .ToImmutableDictionary(g => g.Key, g => g.Sum())
    };
}