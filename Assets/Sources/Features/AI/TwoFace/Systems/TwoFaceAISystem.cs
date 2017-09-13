namespace Assets.Sources.Features.AI.TwoFace.Systems
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Combat;
	using Config;
	using Entitas;
	using Extensions;
	using Helpers;
	using Helpers.Graphs.Pathfinding;
	using Helpers.Graphs.Pathfinding.Heuristics;
	using Helpers.Map;
	using Helpers.MapGen;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.Input)] // TODO: what phase should this be? ProcessActions is too late
	public class TwoFaceAISystem : IExecuteSystem, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;
		private readonly IGroup<GameEntity> entitiesGroup;
		private readonly IGroup<GameEntity> playersGroup;
		private readonly IPathfinder<IntVector2> pathfinder;
		private EntityMap map;
		private readonly Config config;
		private Random random = new Random();

		public TwoFaceAISystem(Contexts contexts)
		{
			gameContext = contexts.game;
			actionsContext = contexts.actions;
			entitiesGroup = gameContext.GetGroup(GameMatcher.TwoFace);
			playersGroup = gameContext.GetGroup(GameMatcher.Player);
			pathfinder = new AStarSearch<IntVector2>(new BasicDistanceHeuristic());
			config = gameContext.GetConfig();
		}

		public void Initialize()
		{
			map = gameContext.GetService<EntityMap>();
		}

		public void Execute()
		{
			if (gameContext.hasTwoFaceState && gameContext.twoFaceState.IsAngry)
			{
				HandleAngry();
			}
			else
			{
				HandleRandomMoves();
			}
		}

		private void HandleRandomMoves()
		{
			foreach (var entity in entitiesGroup.GetEntities())
			{
				if (entity.isActionInProgress)
				{
					continue;
				}

				MoveRandomly(entity);
			}
		}

		private void MoveRandomly(GameEntity entity)
		{
			var pos = entity.position.value;
			var adjacent = pos.GetAdjacentTiles();
			var destination = adjacent.Where(map.IsWalkable).ToList();

			if (destination.Any())
			{
				// actionsContext.BasicMove(entity, destination.GetRandom(random)); TODO: it is way too fast now
			}
		}

		private void HandleAngry()
		{
			var players = playersGroup.GetEntities();

			foreach (var entity in entitiesGroup.GetEntities())
			{
				if (entity.isActionInProgress)
				{
					continue;
				}

				var closestDistance = int.MaxValue;
				var bestMove = new IntVector2();
				var entityPos = entity.position.value;

				var attackTarget = players.FirstOrDefault(x => IntVector2.ManhattanDistance(x.position.value, entityPos) == 1);
				if (attackTarget != null)
				{
					actionsContext.Attack(entity, attackTarget, AttackType.Basic);
					continue;
				}

				foreach (var player in players)
				{
					var playerPos = player.position.value;

					if (IntVector2.ManhattanDistance(entityPos, playerPos) > config.MaxAggroDistance)
					{
						continue;
					}

					var goal = GetGoal(playerPos);

					if (goal == null)
					{
						continue;
					}

					var path = pathfinder.FindPath(map, entityPos, playerPos, goal);
					if (path != null && path.Any() && path.Count <= config.MaxAggroDistance && path.Count < closestDistance)
					{
						closestDistance = path.Count;
						bestMove = path.First();
					}
				}

				if (closestDistance != int.MaxValue)
				{
					actionsContext.BasicMove(entity, bestMove);
				}
				else
				{
					MoveRandomly(entity);
				}
			}
		}

		private Func<IntVector2, IntVector2, bool> GetGoal(IntVector2 goalPosition)
		{
			if (goalPosition.GetAdjacentTiles().FirstOrDefault(map.IsWalkable) != null)
			{
				return (current, end) => IntVector2.ManhattanDistance(current, end) == 1;
			}

			if (goalPosition.GetRadius(2, IntVector2.ManhattanDistance, false).FirstOrDefault(map.IsWalkable) != null)
			{
				return (current, end) => IntVector2.ManhattanDistance(current, end) == 2;
			}

			return null;
		}
	}
}
