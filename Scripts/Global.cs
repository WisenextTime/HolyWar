using System;
using System.Collections.Generic;

using HolyWar.Core;

namespace HolyWar;

public class Globals
{
    internal Globals()
    {
        Terrains = Index.Terrains.PreloadIndexTerrains();
    }

    public static Globals Global => field ??= new Globals();

    [Obsolete("Use DataRegisters.Terrains instead")]
    public Dictionary<string, Terrain> Terrains { get; }

    //TODO
    public static List<string> RiverNames { get; } = [];
}