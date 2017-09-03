namespace Assets.Sources.Features.AI.SheepAndWolf
{
	using System;
	using System.Linq;
	using Combat;
	using Extensions;
	using Helpers.Map;
	using Entitas;
	using Helpers.Graphs.Pathfinding;
	using Helpers.Graphs.Pathfinding.Heuristics;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.Input)] // TODO: what phase should this be? ProcessActions is too late
	public class SheepAiSystem : IExecuteSystem, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;
		private readonly IGroup<GameEntity> group;
		private readonly IGroup<GameEntity> entityGroup;
		private readonly IPathfinder<IntVector2> pathfinder;
		private EntityMap map;

		public SheepAiSystem(Contexts contexts)
		{
			gameContext = contexts.game;
			actionsContext = contexts.actions;
			group = gameContext.GetGroup(GameMatcher.WolfAI);
			entityGroup = contexts.game.GetGroup(GameMatcher.SheepAI);
			pathfinder = new AStarSearch<IntVector2>(new BasicDistanceHeuristic());
		}

		public void Initialize()
		{
			map = gameContext.GetService<EntityMap>();
		}

		public void Execute()
		{
			var wolfPos = group.GetEntities().First().position.value;
			Func<IntVector2, IntVector2, bool> goal = null;

			if (wolfPos.GetAdjacentTiles().FirstOrDefault(map.IsWalkable) != null)
			{
				goal = (current, end) => IntVector2.ManhattanDistance(current, end) == 1;
			} else if (wolfPos.GetRadius(2, IntVector2.ManhattanDistance, false).FirstOrDefault(map.IsWalkable) != null)
			{
				goal = (current, end) => IntVector2.ManhattanDistance(current, end) == 2;
			}

			foreach (var entity in entityGroup.GetEntities())
			{
				if (entity.isActionInProgress)
				{
					continue;
				}

				entity.isShouldAct = false;
				var currentPos = entity.position.value;

				if (IntVector2.ManhattanDistance(currentPos, wolfPos) == 1)
				{
					actionsContext.Attack(entity, group.GetEntities().First(), AttackType.Basic);
					continue;
				}

				if (goal != null)
				{
					var path = pathfinder.FindPath(map, currentPos, wolfPos, goal);
					if (path != null && path.Any())
					{
						actionsContext.BasicMove(entity, path.First());
					}
				}
			}
		}
	}
}