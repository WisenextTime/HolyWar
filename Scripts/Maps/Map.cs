using System.Collections.Generic;
using System.Linq;

using Godot;

using HolyWar.Core;

using KirisameLib.Extensions;

namespace HolyWar.Maps;

public class Map(int width, int height, NewTerrain defaultTerrain)
{
    #region Getter

    public Map(Vector2I size, NewTerrain defaultTerrain) : this(size.X, size.Y, defaultTerrain) { }

    #endregion


    #region Basic Members

    public Vector2I Size => new(width, height);

    private MapTile[,] _tiles = new MapTile[width, height];
    private Dictionary<MapTile, Vector2I> _tilePosDict = new();

    public MapTile this[int x, int y] => GetTile(x, y);
    public MapTile this[Vector2I pos] => GetTile(pos);
    public Vector2I this[MapTile tile] => GetTilePos(tile);

    public MapTile GetTile(Vector2I pos) => GetTile(pos.X, pos.Y);

    public MapTile GetTile(int x, int y)
    {
        if (_tiles[x, y] is not null) return _tiles[x, y];
        var tile = _tiles[x, y] = MapTile.FromTerrain(defaultTerrain);
        _tilePosDict[tile] = new(x, y);
        return tile;
    }

    public Vector2I GetTilePos(MapTile tile) => _tilePosDict[tile];
    public bool TryGetTilePos(MapTile tile, out Vector2I pos) => _tilePosDict.TryGetValue(tile, out pos);

    #endregion


    #region Collections

    public IEnumerable<Vector2I> Coordinates =>
        from x in Enumerable.Range(0, Size.X)
        from y in Enumerable.Range(0, Size.Y)
        select new Vector2I(x, y);
    public IEnumerable<MapTile> Tiles => Coordinates.Select(pos => this[pos]);
    public IEnumerable<(Vector2I coord, MapTile tile)> TilesWithCoord => Coordinates.Select(pos => (pos, this[pos]));

    #endregion


    #region Positional Methods

    public IEnumerable<MapTile> GetNeighbors(int x, int y) => GetNeighborsPos(x, y).Select(p => this[p]);
    public IEnumerable<MapTile> GetNeighbors(Vector2I pos) => GetNeighborsPos(pos).Select(p => this[p]);
    public IEnumerable<MapTile> GetNeighbors(MapTile tile) => GetNeighborsPos(tile).Select(p => this[p]);


    public IEnumerable<Vector2I> GetNeighborsPos(Vector2I pos) => GetNeighborsPos(pos.X, pos.Y);
    public IEnumerable<Vector2I> GetNeighborsPos(MapTile tile) => GetNeighborsPos(GetTilePos(tile));

    public IEnumerable<Vector2I> GetNeighborsPos(int x, int y)
    {
        yield return new Vector2I((x + Size.X - 1) % Size.X, y);
        yield return new Vector2I((x + 1) % Size.X,          y);

        int shift = Size.X + (y % 2 * 2 - 1);
        if (y > 0)
        {
            yield return new Vector2I(x,                    y - 1);
            yield return new Vector2I((x + shift) % Size.X, y - 1);
        }
        if (y + 1 < Size.Y)
        {
            yield return new Vector2I(x,                    y + 1);
            yield return new Vector2I((x + shift) % Size.X, y + 1);
        }
    }

    #endregion
}