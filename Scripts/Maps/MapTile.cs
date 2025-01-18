using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using HolyWar.Core;
using HolyWar.Registers;

namespace HolyWar.Maps;

public class MapTile
{
    #region Fields & Properties

    //todo: 这里有个问题，覆盖的Feature和其他Feature之间的关系怎么处理，是否可以把会覆盖的Feature单独写一个字段，搞清楚这个之前不太方便继续重构
    //      解决了，用优先级。
    private Terrain MainTerrain { get; set; } = DataRegisters.Terrains["Void"];

    private readonly List<TerrainFeature> _features = [];
    public ReadOnlyCollection<TerrainFeature> Features => _features.AsReadOnly();

    public Terrain.TileType TileType => MainTerrain.Type;

    private float _defenseBonusAdd = 0;
    public float DefenseBonus => MainTerrain.DefenseBonus + _defenseBonusAdd;

    private int _movementCostAdd = 0;
    public int MovementCost => MainTerrain.MovementCost + _movementCostAdd;

    public bool IsWater => TileType == Terrain.TileType.Water; // || MainTerrain is LargeRiver;
    public bool IsLand => TileType == Terrain.TileType.Land;

    #endregion


    #region Public Methods

    public void AddTerrainFeature(TerrainFeature feature)
    {
        _features.Add(feature);
        UpdateFeatures();
    }

    public bool RemoveTerrainFeature(TerrainFeature feature)
    {
        if (!_features.Remove(feature)) return false;
        UpdateFeatures();
        return true;
    }

    #endregion


    #region Inner Methods

    private void UpdateFeatures()
    {
        //更新方法你自己写吧x，考虑到有浮点量一直加减加减可能会误差累计，我觉得干脆每次更新后直接重算一遍比较好，喵~
        throw new NotImplementedException();
    }

    #endregion
}