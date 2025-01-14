using System.Collections.Generic;
using HolyWar.Scripts.Core;

namespace HolyWar.Scripts;

public class Globals
{
	internal Globals()
	{
		Terrains = Index.Terrains.PreloadIndexTerrains();
	}
	public static Globals Global => field ??= new Globals();

	public Dictionary<string, Terrain> Terrains;

	//TODO
	public static readonly List<string> RiverNames = [];
}