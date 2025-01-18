namespace HolyWar.Maps.Generation;

public class CommonMapBuilder(int size, int seed = -1) : MapBuilder(size, seed)
{
    private Map _map = new Map(size);

    public override Map GetMap()
    {
        throw new System.NotImplementedException();
    }
}