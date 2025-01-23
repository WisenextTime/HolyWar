using System.Numerics;

namespace HolyWar.Maps;

public readonly record struct TileProperties() : IAdditionOperators<TileProperties, TileProperties, TileProperties>
{
    private int MovementCost { get; init; }
    private float DefenseBonus { get; init; }

    public static TileProperties operator +(TileProperties a, TileProperties b) => new()
    {
        MovementCost = a.MovementCost + b.MovementCost,
        DefenseBonus = a.DefenseBonus + b.DefenseBonus,
    };
}