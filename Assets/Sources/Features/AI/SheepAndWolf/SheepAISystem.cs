using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using System.Linq;

public sealed class SheepAISystem : IExecuteSystem
{
    GameContext context;
	private ActionsContext actionsContext;
    IGroup<GameEntity> group;
	private IGroup<GameEntity> entityGroup;

	public SheepAISystem(Contexts contexts)// : base(contexts.game)
    {
        context = contexts.game;
	    actionsContext = contexts.actions;
        group = context.GetGroup(GameMatcher.WolfAI);
	    entityGroup = context.GetGroup(GameMatcher.SheepAI);
    }

    /*protected override void Execute(List<GameEntity> entities)
    {

        foreach (var entity in entities)
        {
            entity.isShouldAct = false;
            entity.isActionInProgress = true;

            var wolfPos = group.GetEntities().First().position.value;
            var currentPos = entity.position.value;
            var moves = currentPos.GetAdjacentTiles().Where(x => Map.Instance.IsWalkable(x));

            if (moves.Count() != 0)
            {
                var best = currentPos;
                foreach (var move in moves)
                {
                    if (IntVector2.ManhattanDistance(move, wolfPos) >= IntVector2.ManhattanDistance(best, wolfPos))
                    {
                        best = move;
                    }
                }

	            var relativeDirection = best - currentPos;
	            actionsContext.CreateBasicMove(entity, relativeDirection);
	            //entity.ReplacePosition(best, true);
            }
            else
            {
                entity.isActionInProgress = false;
                context.CreateEntity().AddAction(ActionType.NOTHING, new NothingArgs() { source = entity });
                UnityEngine.Debug.Log("Nothing");
            }
        }
    }*/

    /*protected override bool Filter(GameEntity entity)
    {
        return entity.isSheepAI && entity.isAI && !entity.isActionInProgress;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
		// return context.CreateCollector(GameMatcher.ShouldAct); TODO
		return context.CreateCollector(GameMatcher.);
	}*/

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
			var moves = currentPos.GetAdjacentTiles().Where(x => Map.Instance.IsWalkable(x));

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

				var relativeDirection = best - currentPos;
				actionsContext.CreateBasicMove(entity, relativeDirection);
				//entity.ReplacePosition(best, true);
			}
			else
			{
				entity.isActionInProgress = false;
				context.CreateEntity().AddAction(ActionType.NOTHING, new NothingArgs() { source = entity });
				UnityEngine.Debug.Log("Nothing");
			}
		}
	}
}