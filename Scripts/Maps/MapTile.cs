using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using HolyWar.Core;
using HolyWar.Registers;

using KirisameLib.Collections;

namespace HolyWar.Maps;

public class MapTile
{
    #region Fields & Properties

    private Terrain MainTerrain
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            _propertiesDirty = true;
        }
    } = DataRegisters.Terrains["Void"];

    private bool _propertiesDirty = true;

    private readonly List<TerrainFeature> _features = [];
    private readonly List<TerrainFeature> _overwritingFeatures = [];
    public int OverwritingPriority { get; set; }
    public CombinedListView<TerrainFeature> Features => field ??= new(_features, _overwritingFeatures);
    public IEnumerable<TerrainFeature> EffectiveFeatures =>
        _overwritingFeatures.Count == 0 ? Features : Features.Where(f => f.Priority >= OverwritingPriority);


    public Terrain.TileType TileType => MainTerrain.Type;

    public float DefenseBonus
    {
        get
        {
            if (_propertiesDirty) UpdateProperties();
            return field;
        }
        private set;
    }
    public int MovementCost
    {
        get
        {
            if (_propertiesDirty) UpdateProperties();
            return field;
        }
        private set;
    }

    public bool IsWater => TileType == Terrain.TileType.Water; // || MainTerrain is LargeRiver;
    public bool IsLand => TileType == Terrain.TileType.Land;

    #endregion


    #region Public Methods

    public void AddTerrainFeature(TerrainFeature feature)
    {
        if (!feature.Overwriting) _features.Add(feature);
        else
        {
            _overwritingFeatures.Add(feature);
            if (feature.Priority > OverwritingPriority) OverwritingPriority = feature.Priority;
        }

        _propertiesDirty = true;
    }

    public bool RemoveTerrainFeature(TerrainFeature feature)
    {
        bool result;
        if (!feature.Overwriting) result = _features.Remove(feature);
        else
        {
            result = _overwritingFeatures.Remove(feature);
            if (result) OverwritingPriority = _overwritingFeatures.Max(f => f.Priority);
        }

        if (result) _propertiesDirty = true;
        return true;
    }

    #endregion


    #region Inner Methods

    private void UpdateProperties()
    {
        //todo:其他属性还没做x
        _propertiesDirty = false;

        (MovementCost, DefenseBonus) = _overwritingFeatures.Count == 0
            ? (MainTerrain.MovementCost, MainTerrain.DefenseBonus)  //todo: 也许这里用个析构函数之类的东西获取元组会更好
            : (0, 0);

        foreach (var feature in EffectiveFeatures)
        {
            MovementCost += feature.MovementCost;
            DefenseBonus += feature.DefenseBonus;
        }
    }

    #endregion
}