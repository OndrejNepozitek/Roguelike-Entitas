namespace Assets.Sources.Features.Stats
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Scripts;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	public class ShouldDieSystem : ReactiveSystem<GameEntity>, ICleanupSystem
	{
		private readonly GameContext context;
		private readonly IGroup<GameEntity> group;

		public ShouldDieSystem(Contexts contexts) : base(contexts.game)
		{
			context = contexts.game;
			group = context.GetGroup(GameMatcher.Dead);
		}

		public void Cleanup()
		{
			foreach (var entity in group.GetEntities())
			{
				if (entity.isDead)
				{
					entity.Destroy();
				}
			}
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity.health.Value <= 0)
				{
					// TODO: should be done better
					if (!NetworkController.Instance.IsMultiplayer && entity.hasPlayer)
					{
						context.GetService<GameController>().GameOver();
						continue;
					}

					entity.isDead = true;
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return true;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Health);
		}
	}
}