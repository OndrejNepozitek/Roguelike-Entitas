namespace Assets.Sources.Features.AI.SheepAndWolf
{
	using System.Linq;
	using Extensions;
	using Helpers.Map;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.ProcessActions)]
	public class SheepAiSystem : IExecuteSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;
		private readonly IGroup<GameEntity> group;
		private readonly IGroup<GameEntity> entityGroup;

		public SheepAiSystem(Contexts contexts)
		{
			gameContext = contexts.game;
			actionsContext = contexts.actions;
			group = gameContext.GetGroup(GameMatcher.WolfAI);
			entityGroup = contexts.game.GetGroup(GameMatcher.SheepAI);
		}

		public void Execute()
		{
			foreach (var entity in entityGroup.GetEntities())
			{
				if (entity.isActionInProgress)
				{
					continue;
				}

				entity.isShouldAct = false;
				//entity.isActionInProgress = true; TODO: who should handle this?

				var wolfPos = group.GetEntities().First().position.value;
				var currentPos = entity.position.value;
				var moves = currentPos.GetAdjacentTiles().Where(x => gameContext.GetService<EntityMap>().IsWalkable(x));

				if (moves.Count() != 0)
				{
					var best = currentPos;
					foreach (var move in moves)
					{
						if (IntVector2.ManhattanDistance(move, wolfPos) <= IntVector2.ManhattanDistance(best, wolfPos))
						{
							best = move;
						}
					}

					if (best != currentPos)
					{
						actionsContext.BasicMove(entity, best);
					}
				
					//entity.ReplacePosition(best, true);
				}
				else
				{
					entity.isActionInProgress = false;
					/*context.CreateEntity().AddAction(ActionType.NOTHING, new NothingArgs() { source = entity });
				UnityEngine.Debug.Log("Nothing");*/
				}
			}
		}
	}
}