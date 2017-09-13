namespace Assets.Sources.Features.Stats.Systems
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Entitas;
	using Extensions;
	using Helpers;
	using Helpers.Map;
	using Helpers.MapGen;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Scripts;

	[ExecutePhase(ExecutePhase.ProcessActions)]
	public class RespawnPositionSystem : ReactiveSystem<ActionsEntity>
	{
		private readonly GameContext gameContext;
		private readonly IGroup<GameEntity> players;
		private EntityMap map;

		public RespawnPositionSystem(Contexts contexts) : base(contexts.actions)
		{
			gameContext = contexts.game;
			players = gameContext.GetGroup(GameMatcher.Player);
		}

		public void Initialize()
		{
			map = gameContext.GetService<EntityMap>();
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action);
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction && entity.action.Action is RespawnAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			map = gameContext.GetService<EntityMap>(); // TODO: remove

			foreach (var rawAction in entities)
			{
				var action = (RespawnAction)rawAction.action.Action;
				var firstPlayer = players.GetEntities().FirstOrDefault();

				if (firstPlayer == null)
				{
					return;
				}

				var positionsToChooseFrom = firstPlayer.position.value.GetRadius(4, IntVector2.ManhattanDistance, true);
				var position = positionsToChooseFrom.Where(map.IsInBorders).Where(map.IsWalkable).GetRandom(new Random());

				action.Position = position;
			}
		}
	}
}