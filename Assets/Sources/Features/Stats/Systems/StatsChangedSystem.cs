namespace Assets.Sources.Features.Stats.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Scripts;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	public class StatsChangedSystem : ReactiveSystem<GameEntity>, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private InventoryController inventoryController;

		public StatsChangedSystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			inventoryController = gameContext.GetService<InventoryController>();
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Stats.Added(), GameMatcher.Health.Added());
		}

		protected override bool Filter(GameEntity entity)
		{
			return true;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity == gameContext.GetCurrentPlayer())
				{
					if (entity.hasHealth)
					{
						inventoryController.SetHealth(entity.health.Value);
					}

					break;
				}
			}
		}
	}
}