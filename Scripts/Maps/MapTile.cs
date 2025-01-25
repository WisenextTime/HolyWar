using System.Collections.Generic;
using System.Linq;

using KirisameLib.Collections;

namespace HolyWar.Maps;

public class MapTile
{
    #region Fields & Properties

    private NewTerrain MainTerrain
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            _propertiesDirty = true;
        }
    } //= DataRegisters.Terrains["Void"]; (todo: add this

    private readonly List<NewTerrainFeature> _features = [];
    private readonly List<NewTerrainFeature> _overwritingFeatures = [];
    public int OverwritingPriority { get; set; }
    public CombinedListView<NewTerrainFeature> Features => field ??= new(_features, _overwritingFeatures);
    public IEnumerable<NewTerrainFeature> EffectiveFeatures =>
        _overwritingFeatures.Count == 0 ? Features : Features.Where(f => f.Priority >= OverwritingPriority);


    public TerrainType TerrainType => MainTerrain.Type;

    private bool _propertiesDirty = true;
    public TileProperties Properties
    {
        get
        {
            if (_propertiesDirty) UpdateProperties();
            return field;
        }
        private set;
    }
    public TerrainProperties TerrainProperties
    {
        get
        {
            if (_propertiesDirty) UpdateProperties();
            return field;
        }
        private set;
    }

    public bool IsWater => TerrainType == TerrainType.Water; // || MainTerrain is LargeRiver;
    public bool IsLand => TerrainType == TerrainType.Land;

    #endregion


    #region Public Methods

    public void AddTerrainFeature(NewTerrainFeature feature)
    {
        if (!feature.Overwriting) _features.Add(feature);
        else
        {
            _overwritingFeatures.Add(feature);
            if (feature.Priority > OverwritingPriority) OverwritingPriority = feature.Priority;
        }

        _propertiesDirty = true;
    }

    public bool RemoveTerrainFeature(NewTerrainFeature feature)
    {
        bool result;
        if (!feature.Overwriting) result = _features.Remove(feature);
        else
        {
            result = _overwritingFeatures.Remove(feature);
            if (result) OverwritingPriority = _overwritingFeatures.Max(f => f.Priority);
        }

        if (result) _propertiesDirty = true;
        return result;
    }

    #endregion


    #region Inner Methods

    private void UpdateProperties()
    {
        _propertiesDirty = false;

        TileProperties properties = _overwritingFeatures.Count == 0 ? MainTerrain.TileProperties : new();
        TerrainProperties terrainProperties = new();
        foreach (NewTerrainFeature feature in EffectiveFeatures)
        {
            properties += feature.TileProperties;
            terrainProperties += feature.TerrainProperties;
        }

        (Properties, TerrainProperties) = (properties, terrainProperties);
    }

    #endregion
}