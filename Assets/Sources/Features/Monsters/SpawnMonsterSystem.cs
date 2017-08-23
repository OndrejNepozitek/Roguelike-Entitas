namespace Assets.Sources.Features.Monsters
{
	using System.Collections.Generic;
	using Actions;
	using Entitas;
	using Helpers.Map;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using MapTracker;

	[ExecutePhase(ExecutePhase.ReactToActions)]
	[DependsOn(typeof(ActionsFeature), typeof(MapTrackerSystem))]
	public class SpawnMonsterSystem : ReactiveSystem<ActionsEntity>
	{
		private readonly GameContext gameContext;

		public SpawnMonsterSystem(Contexts contexts) : base(contexts.actions)
		{
			gameContext = contexts.game;
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action.Added());
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction && entity.action.Action is SpawnMonsterAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var rawAction in entities)
			{
				var action = (SpawnMonsterAction) rawAction.action.Action;

				if (!gameContext.GetService<EntityMap>().IsWalkable(action.Position))
				{
					rawAction.Destroy(); // TODO: same problem as destroying movement actions
					continue;
				}

				gameContext.CreateMonster(action.Position, action.Type, action.Entity);
			}
		}
	}
}