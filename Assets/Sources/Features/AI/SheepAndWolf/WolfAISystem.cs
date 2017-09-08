namespace Assets.Sources.Features.AI.SheepAndWolf
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Helpers.Map;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.ProcessActions)]
	public class WolfAiSystem : ReactiveSystem<GameEntity>, IInitializeSystem
	{
		private readonly GameContext gameContext;
		private readonly IGroup<GameEntity> group;
		private EntityMap map;

		public WolfAiSystem(Contexts contexts) : base(contexts.game)
		{
			gameContext = contexts.game;
			group = gameContext.GetGroup(GameMatcher.SheepAI);
		}

		public void Initialize()
		{
			map = gameContext.GetService<EntityMap>();
		}

		protected override void Execute(List<GameEntity> entities)
		{
			if (entities.Count > 1)
				throw new InvalidOperationException();

			foreach (var entity in entities)
			{
				entity.isShouldAct = false;
				entity.isActionInProgress = true;

				var currentPos = entity.position.value;
				var moves = currentPos.GetAdjacentTiles().Where(x => map.IsWalkable(x));

				foreach (var pos in currentPos.GetAdjacentTiles())
				{
					var tile = map.GetEntitiesOnTile(pos);
					if (tile == null)
						continue;

					foreach (var sheep in tile)
					{
						if (sheep.isSheepAI)
						{
							/* gameContext.CreateEntity().AddAction(ActionType.ATTACK, new AttackArgs() { source = entity, target = sheep, amount = 150 });
							entity.isActionInProgress = false; */
							return;
						}
					}
				}

				if (moves.Count() != 0)
				{
					var best = moves.ElementAt(0);
					var distance = float.MaxValue;

					foreach (var sheep in group.GetEntities())
					{
						foreach (var move in moves)
						{
							if (IntVector2.ManhattanDistance(move, sheep.position.value) < distance)
							{
								best = move;
								distance = IntVector2.ManhattanDistance(move, sheep.position.value);
							}
						}
					}

					entity.ReplacePosition(best, true);
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isWolfAI && entity.isAI;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.ShouldAct);
		}
	}
}