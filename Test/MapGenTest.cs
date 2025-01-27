using Godot;

using HolyWar.Game;
using HolyWar.Maps;
using HolyWar.Maps.Generation;
using HolyWar.Registers;

#if TOOLS

namespace HolyWar.Test;

public partial class MapGenTest : Node
{
    private GameCamera Camera => field ??= GetNode<GameCamera>("GameCamera");

    public override void _Ready()
    {
        DataPreloading.PreloadTerrains();

        Vector2I mapSize = new(80, 25);
        var seed = (int)Time.GetTicksMsec();
        // var seed = 2676;
        GD.Print($"seed: {seed}");
        Map map = new CommonMapBuilder(mapSize, seed)
                 .InitLandWater(DataRegisters.Terrains["Grassland"], DataRegisters.Terrains["Coast"],
                                DataRegisters.Terrains["Ocean"],     DataRegisters.Terrains["Lake"])
                 .GetMap();
        GameMap gameMap = new GameMap(map);
        AddChild(gameMap);

        // Camera.CameraUpEdge = (float)mapSize.Y / 2 * 0.75f;
        // Camera.CameraDownEdge = -(float)mapSize.Y / 2 * 0.75f;
        // Camera.CameraLeftEdge = -(float)mapSize.X / 2 * 1.73205f;
        // Camera.CameraRightEdge = (float)mapSize.X / 2 * 1.73205f;
        Camera.CameraLeftEdge = mapSize.X * 0.5f * 1.73205f;
        Camera.CameraRightEdge = mapSize.X * 1.5f * 1.73205f;
        Camera.CameraUpEdge = mapSize.Y * 1.5f;
        Camera.CameraDownEdge = 0;
        Camera.ClampPosition();
    }
}

#endif