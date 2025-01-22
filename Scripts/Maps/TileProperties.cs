using System.Numerics;

namespace HolyWar.Maps;

public readonly record struct TileProperties(
    int MovementCost,
    float DefenseBonus
) : IAdditionOperators<TileProperties, TileProperties, TileProperties>
{
    public static TileProperties operator +(TileProperties a, TileProperties b) =>
        new(a.MovementCost + b.MovementCost,
            a.DefenseBonus + b.DefenseBonus);
}