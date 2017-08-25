namespace Assets.Sources.Extensions
{
	using System;
	using Features.Combat;
	using Features.Items;
	using Features.Items.Actions;
	using Features.Monsters;
	using Features.Movement;
	using Helpers.Monsters;

	public static class ActionsContextExtensions
	{
		public static ActionsEntity BasicMove(this ActionsContext context, GameEntity targetEntity, IntVector2 direction)
		{
			var entity = context.CreateEntity();

			if (!targetEntity.hasNetworkTracked)
			{
				throw new ArgumentException("Target entity must be network tracked when used in actions");
			}

			entity.AddAction(new BasicMoveAction() { Position = direction, Entity = targetEntity.GetReference() });

			return entity;
		}

		public static ActionsEntity SpawnItem(this ActionsContext context, IItem item, IntVector2 position)
		{
			return context.SpawnItem(item.Name, position);
		}

		public static ActionsEntity SpawnItem(this ActionsContext context, ItemName name, IntVector2 position)
		{
			var entity = context.CreateEntity();

			entity.AddAction(new SpawnItemAction() { Item = name, Position = position });

			return entity;
		}

		public static ActionsEntity SpawnMonster(this ActionsContext context, MonsterType type, IntVector2 position)
		{
			var entity = context.CreateEntity();

			entity.AddAction(new SpawnMonsterAction() { Type = type, Position = position, LootSeed = UnityEngine.Random.Range(0, int.MaxValue) });

			return entity;
		}

		public static ActionsEntity Equip(this ActionsContext context, IItem item, GameEntity target)
		{
			var entity = context.CreateEntity();

			entity.AddAction(new EquipAction() { Item = item.Name, Entity = target.GetReference() });

			return entity;
		}

		public static ActionsEntity PickAndEquip(this ActionsContext context, IntVector2 position, GameEntity target)
		{
			var entity = context.CreateEntity();

			entity.AddAction(new PickAndEquipAction() { Position = position, Entity = target.GetReference() });

			return entity;
		}

		public static ActionsEntity Attack(this ActionsContext context, GameEntity source, GameEntity target, AttackType type)
		{
			var entity = context.CreateEntity();

			entity.AddAction(new AttackAction(){ Source = source.GetReference(), Target = target.GetReference(), Type = type });

			return entity;
		}
	}
}