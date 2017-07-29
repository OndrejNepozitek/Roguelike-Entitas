using System.Runtime.CompilerServices;
using Entitas;

public static class GameContextExtensions
{
    public static GameEntity CreateWall(this GameContext context, IntVector2 pos, string prefab)
    {
        var entity = context.CreateEntity();

        entity.isMapTile = true;
        entity.isSolid = true;
        entity.isWall = true;

        entity.AddPosition(pos, false);
        entity.AddAsset(prefab);

        return entity;
    }

    public static GameEntity CreateFloor(this GameContext context, IntVector2 pos, string prefab)
    {
        var entity = context.CreateEntity();

        entity.isMapTile = true;
        entity.isFloor = true;

        entity.AddPosition(pos, false);
        entity.AddAsset(prefab);

        return entity;
    }

    public static GameEntity CreateTorch(this GameContext context, IntVector2 pos)
    {
        var entity = context.CreateEntity();

        entity.isSolid = true;

        entity.AddPosition(pos, false);
        entity.AddAsset(Prefabs.TORCH);
        entity.AddLight(5);

        return entity;
    }

	public static void AddAction(this GameEntity entity, ActionType type, IEventArgs args)
	{
		entity.AddActionOld(type, args); // TODO: remove
	}
}