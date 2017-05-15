using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using System.Linq;

public sealed class SheepAISystem : ReactiveSystem<GameEntity>
{
    GameContext context;
    IGroup<GameEntity> group;

    public SheepAISystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
        group = context.GetGroup(GameMatcher.WolfAI);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (entities.Count > 1)
            throw new InvalidOperationException();

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

                entity.ReplaceSmoothMovement(best, 0.3f);
            }
            else
            {
                entity.isActionInProgress = false;
                context.CreateEntity().AddAction(ActionType.NOTHING, new NothingArgs() { source = entity });
                UnityEngine.Debug.Log("Nothing");
            }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isSheepAI;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ShouldAct);
    }
}