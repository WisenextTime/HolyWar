using Godot;

using HolyWar.Core;

using KirisameLib.Data.Register;
using KirisameLib.Extensions;

namespace HolyWar.Registers;

public static class DataRegisters
{


    public static CommonRegister<Terrain> Terrains { get; } = new(_ => Terrain.Default);
}