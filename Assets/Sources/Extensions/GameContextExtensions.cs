using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Assets.Sources.Features.Items;
using Assets.Sources.Helpers;
using Assets.Sources.Helpers.Items;
using Assets.Sources.Helpers.Monsters;
using Assets.Sources.Helpers.Networking;
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

	public static GameEntity CreatePlayer(this GameContext context, IntVector2 pos, bool focus, Player player)
	{
		var entity = context.CreateEntity();
		
		entity.AddPlayer(focus, player.Id);
		entity.AddPosition(pos, false);
		entity.isTurnBased = true;
		entity.isInit = true;
		entity.isSolid = true;
		entity.AddAsset(Prefabs.BodyBrown);
		entity.AddStats(30, 100, 10, 1);
		entity.AddHealth(100);
		entity.isWolfAI = true;
		//entity.isAI = true;
		entity.AddName(player.Name);
		entity.AddRevealAround(5);
		entity.AddLight(5);
		entity.isShouldAct = true;
		entity.AddInventory(new Dictionary<InventorySlot, InventoryItem>());
		entity.AddNetworkTracked(null);

		return entity;
	}

	public static GameEntity CreateMonster(this GameContext context, IntVector2 pos, MonsterType type, EntityReference reference)
	{
		var monsters = context.GetService<MonsterDatabase>();
		var config = monsters.GetItem(type);

		var entity = context.CreateEntity();

		entity.AddPosition(pos, false);
		entity.isTurnBased = true;
		entity.isInit = true;
		entity.isSolid = true;
		entity.isAI = true;
		entity.isShouldAct = true;
		entity.isAttackable = true;
		entity.AddNetworkTracked(reference);

		entity.AddAsset(config.Prefab);
		entity.AddStats(config.Attack, config.AttackSpeed, config.Defense, config.MovementSpeed);
		entity.AddHealth(config.Health);
		entity.isSheepAI = config.Sheep;

		EntityDatabase.Instance.AddEntity(reference.Id, entity);

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

	public static T GetService<T>(this GameContext context)
	{
		return context.databases.Databases.GetItem<T>();
	}

	public static void AddService<T>(this GameContext context, T service)
	{
		context.databases.Databases.AddItem(service);
	}

	public static GameEntity CreateItem(this GameContext context, ItemName name, IntVector2 pos)
	{
		var items = context.GetService<ItemDatabase>();
		var item = items.GetItem(name);
		return context.CreateItem(item, pos);
	}

	public static void AddAction(this GameEntity entity, ActionType type, IEventArgs args)
	{
		entity.AddActionOld(type, args); // TODO: remove
	}

	public static GameEntity GetCurrentPlayer(this GameContext context)
	{
		return context.currentPlayer.Entity.GetEntity();
	}
}