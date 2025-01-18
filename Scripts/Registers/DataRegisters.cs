using Godot;

using HolyWar.Core;

using KirisameLib.Data.Register;
using KirisameLib.Extensions;

namespace HolyWar.Registers;

public static class DataRegisters
{
    public static CommonRegister<Terrain> TerrainsRegister { get; } = new(_ => Terrain.Default);
    public static CommonRegister<TerrainFeature> TerrainFeaturesRegister { get; } = new(_ => TerrainFeature.Default);

    public static IRegister<Terrain> Terrains => TerrainsRegister;
    public static IRegister<TerrainFeature> TerrainsFeatures => TerrainFeaturesRegister;
}