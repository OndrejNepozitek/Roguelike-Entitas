using System.Collections.Generic;
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
        entity.AddAsset(Prefabs.Torch);
        entity.AddLight(5);

        return entity;
    }

	public static GameEntity CreatePlayer(this GameContext context, IntVector2 pos, bool focus, string name)
	{
		var entity = context.CreateEntity();
		
		entity.AddPlayer(focus);
		entity.AddPosition(pos, false);
		entity.isTurnBased = true;
		entity.isInit = true;
		entity.isSolid = true;
		entity.AddAsset(Prefabs.BodyBrown);
		entity.AddStats(30, 100, 10, 1);
		entity.AddHealth(100);
		entity.isWolfAI = true;
		//entity.isAI = true;
		entity.AddName(name);
		entity.AddRevealAround(5);
		entity.AddLight(5);
		entity.isShouldAct = true;
		entity.AddInventory(new Dictionary<InventorySlot, InventoryItem>());
		entity.AddNetworkTracked(null);
		Map.Instance.AddEntity(entity, pos);

		return entity;
	}

	public static GameEntity CreateMonster(this GameContext context, IntVector2 pos, MonsterType type, EntityReference reference)
	{
		var config = MonsterDatabase.Instance.GetMonster(type);

		var entity = context.CreateEntity();

		entity.AddPosition(pos, false);
		entity.isTurnBased = true;
		entity.isInit = true;
		entity.isSolid = true;
		entity.isAI = true;
		entity.isShouldAct = true;
		entity.AddNetworkTracked(reference);
		
		entity.AddAsset(config.Prefab);
		entity.AddStats(config.Attack, config.AttackSpeed, config.Defense, config.MovementSpeed);
		entity.AddHealth(config.Health);
		entity.isSheepAI = config.Sheep;

		Map.Instance.AddEntity(entity, pos);

		return entity;
	}

	public static GameEntity CreateItem(this GameContext context, IItem item, IntVector2 pos)
	{
		var entity = context.CreateEntity();

		entity.AddPosition(pos, false);
		entity.AddItem(item);
		entity.isInit = true;
		entity.AddAsset(item.Prefab);

		return entity;
	}

	public static GameEntity CreateItem(this GameContext context, ItemName name, IntVector2 pos)
	{
		var item = ItemDatabase.Instance.GetItem(name);
		return context.CreateItem(item, pos);
	}

	public static void AddAction(this GameEntity entity, ActionType type, IEventArgs args)
	{
		entity.AddActionOld(type, args); // TODO: remove
	}
}