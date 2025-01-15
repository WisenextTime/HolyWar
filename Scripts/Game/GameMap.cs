using System;
using System.Linq;
using Godot;
using HolyWar.Scripts.Core;
using HolyWar.Scripts.Maps;

namespace HolyWar.Scripts.Game;

public partial class GameMap : Node
{
	public Map Map;
	private static PackedScene TileScene => field ??= ResourceLoader.Load<PackedScene>("res://Scenes/Tile.tscn");

	public static readonly Func<Vector2, Vector3> ToRenderCoord = coord =>
		new Vector3(1.73205f * coord.X + (coord.Y % 2 == 0 ? 0 : 1.73205f / 2), 0, coord.Y * 1.5f);
	

	public void DrawMap()
	{
		if (Map == null) return;
		// for (var x = -Map.Size + 1; x < Map.Size; x++)
		// {
		// 	var y = 0;
		// 	foreach (var tile in Map.Tiles[x + Map.Size - 1])
		// 	{
		// 		var coord = new Vector2(x, y);
		// 		y++;
		// 		var tileScene = TileScene.Instantiate<MeshInstance3D>();
		// 		tileScene.Position = ToRenderCoord(coord);
		// 		tileScene.Mesh = Globals.Global.Terrains[tile.MainTerrain].Mesh;
		// 		AddChild(tileScene);
		// 	}

			//GD.Print(string.Join(',', Map.Tiles[x + Map.Size - 1].Select(t => t.MainTerrain)));
		//}
		foreach (var (pos,tile) in Map.Tiles)
		{
			var tileScene = TileScene.Instantiate<MeshInstance3D>();
			tileScene.Position = ToRenderCoord(pos);
			tileScene.Mesh = Globals.Global.Terrains[tile.MainTerrain].Mesh;
			if (!tile.GetMainTerrain().EdgeRender)
			{
				tileScene.Layers |= 0b_10;
			}
			foreach (var feature in tile.Features)
			{
				//TODO
				if (Globals.Global.Terrains[feature] is River) continue;
				if (Globals.Global.Terrains[feature] is LargeRiver) continue;
				var featureScene = TileScene.Instantiate<MeshInstance3D>();
				featureScene.Mesh = Globals.Global.Terrains[feature].Mesh;
				var featureSource = (TerrainFeature)Globals.Global.Terrains[feature];
				if (featureSource.MaterialSameAs)
				{
					featureScene.MaterialOverride = tileScene.Mesh.SurfaceGetMaterial(0);
				}
				if (!featureSource.EdgeRender)
				{
					featureScene.Layers |= 0b_10;
				}
				tileScene.AddChild(featureScene);
			}
			AddChild(tileScene);
			var copy = (MeshInstance3D)tileScene.Duplicate();
			copy.Position = ToRenderCoord(new Vector2(pos.X + 2 * Map.Size + 2, pos.Y));
			AddChild(copy);
		}
	}
}