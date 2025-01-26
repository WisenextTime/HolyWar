using System;

using Godot;

using HolyWar.Maps;

namespace HolyWar.Game;

public partial class GameMap(Map map) : Node
{
    public Map Map => map;

    public static readonly Func<Vector2, Vector3> ToRenderCoord = coord =>
        new Vector3(1.73205f * coord.X + (coord.Y % 2 == 0 ? 0 : 1.73205f / 2), 0, coord.Y * 1.5f);

    public override void _Ready()
    {
        foreach (var (coord, tile) in map.TilesWithCoord)
        {
            var tileNode = TileNode.Create(tile.MainTerrain.DisplayProperties);
            tileNode.Position = ToRenderCoord(coord);
            AddChild(tileNode);
            foreach (var feature in tile.Features)
            {
                var featureNode = TileNode.Create(feature.DisplayProperties);
                if (feature.SameTexture) featureNode.MaterialOverride = tileNode.Mesh.SurfaceGetMaterial(0);
                tileNode.AddChild(featureNode);
            }

            var copy = (TileNode)tileNode.Duplicate();
            copy.Position = ToRenderCoord(new Vector2(coord.X + Map.Size.X, coord.Y));
            AddChild(copy);
        }
    }
}