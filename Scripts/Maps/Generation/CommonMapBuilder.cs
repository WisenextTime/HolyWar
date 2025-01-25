using Godot;

namespace HolyWar.Maps.Generation;

public class CommonMapBuilder(Vector2I size, int seed = -1) : MapBuilder(size, seed)
{
    private Map _map = new Map(size, new NewTerrain()); //todo: 改掉这个默认地形

    public override Map GetMap()
    {
        throw new System.NotImplementedException();
    }
}