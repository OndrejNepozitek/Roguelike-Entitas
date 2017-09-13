namespace Assets.Sources.Features.Loot.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Extensions;
	using Helpers.Loot;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Stats;
	using Stats.Systems;
	using UnityEngine;
	using View;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	[ExecutesAfter(typeof(ShouldDieSystem))]
	[ExecutesBefore(typeof(AddViewSystem))]
	public class SpawnLootSystem : ReactiveSystem<GameEntity>, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private LootDatabase lootDatabase;
		private EntityMap map;

		public SpawnLootSystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			lootDatabase = gameContext.GetService<LootDatabase>();
			map = gameContext.GetService<EntityMap>();
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Dead.Added());
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasLoot && entity.hasPosition;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				var loot = lootDatabase.GetLoot(entity.loot.GroupName, entity.loot.Seed);

				// Make sure that there is not any item on the tile
				if (loot.HasValue && map.GetItem(entity.position.value) == null)
				{
					gameContext.CreateItem(loot.Value, entity.position.value);
				}
			}
		}
	}
}