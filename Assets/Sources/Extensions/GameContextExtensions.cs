using Entitas;

public static class GameContextExtensions
{
    public static GameEntity CreateWall(this GameContext context, IntVector2 pos, string prefab)
    {
        var entity = context.CreateEntity();

        entity.isMapTile = true;
        entity.isSolid = true;
        entity.isWall = true;

        entity.AddPosition(pos);
        entity.AddAsset(prefab);

        return entity;
    }

    public static GameEntity CreateFloor(this GameContext context, IntVector2 pos, string prefab)
    {
        var entity = context.CreateEntity();

        entity.isMapTile = true;
        entity.isFloor = true;

        entity.AddPosition(pos);
        entity.AddAsset(prefab);

        return entity;
    }
}