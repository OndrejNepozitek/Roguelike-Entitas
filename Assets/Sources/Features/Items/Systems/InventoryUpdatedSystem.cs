namespace Assets.Sources.Features.Items.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Scripts;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	public class InventoryUpdatedSystem : ReactiveSystem<GameEntity>
	{
		private readonly GameContext gameContext;

		public InventoryUpdatedSystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Inventory.Added());
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasInventory;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity == gameContext.GetCurrentPlayer()) // TODO: maybe use id instead?
				{
					gameContext.GetService<InventoryController>().SetInventory(entity.inventory.Items);
					gameContext.GetService<InventoryController>().SetStats(entity.GetModifiedStats());
					break;
				}
			}
		}
	}
}