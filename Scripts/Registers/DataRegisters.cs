using Godot;

using HolyWar.Core;
using HolyWar.Maps;

using KirisameLib.Data.Register;
using KirisameLib.Extensions;

namespace HolyWar.Registers;

public static class DataRegisters
{
    public static CommonRegister<NewTerrain> TerrainsRegister { get; } = new(_ => NewTerrain.Default);
    public static CommonRegister<NewTerrainFeature> TerrainFeaturesRegister { get; } = new(_ => NewTerrainFeature.Default);

    public static IRegister<NewTerrain> Terrains => TerrainsRegister;
    public static IRegister<NewTerrainFeature> TerrainsFeatures => TerrainFeaturesRegister;
}