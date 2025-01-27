namespace HolyWar.Maps;

public readonly record struct TerrainProperties()
{
    public bool IsImpassable { get; init; } = false;
    public bool IsOpen { get; init; } = true;
    public bool IsRough { get; init; } = false;
    public bool CanPreventFreshWater { get; init; } = false;
    public bool IsFreshWater { get; init; } = false;
    public bool IsUnbuildable { get; init; } = false;

    // only for water
    public bool IsCoast { get; init; } = false;
    public bool IsOcean { get; init; } = false;

    public static TerrainProperties operator +(TerrainProperties a, TerrainProperties b) => new()
    {
        IsImpassable = a.IsImpassable || b.IsImpassable,
        IsOpen = a.IsOpen || b.IsOpen, // todo: 确定吗
        IsRough = a.IsRough || b.IsRough,
        CanPreventFreshWater = a.CanPreventFreshWater || b.CanPreventFreshWater,
        IsFreshWater = a.IsFreshWater || b.IsFreshWater,
        IsUnbuildable = a.IsUnbuildable || b.IsUnbuildable,

        IsCoast = a.IsCoast || b.IsCoast,
        IsOcean = a.IsOcean || b.IsOcean,
    };
}