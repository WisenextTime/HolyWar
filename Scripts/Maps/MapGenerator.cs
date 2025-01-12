using Godot;
using Godot.Collections;

namespace HolyWar.Scripts.Maps;

public abstract class MapGenerator(int size = 5,int seed = default)
{
	public int Seed = seed==default ? (int)Time.GetTicksMsec() : seed;
	public int Size = size;
	public abstract Map Generate(Dictionary args);
}

