using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AIJumpSystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public AIJumpSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
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
            var moved = false;
            for (int i = 0; i < 10; i++)
            {
                var pos = currentPos + new Vector2(3 * UnityEngine.Random.Range(-1, 2), 3 * UnityEngine.Random.Range(-1, 2));
                if (Map.Instance.IsWalkable((int)pos.x, (int) pos.y))
                {
                    entity.ReplaceSmoothMovement(pos, 0.3f);
                    moved = true;
                    break;
                }
            }

            if (moved == false)
            {
                context.CreateEntity().AddAction(ActionType.NOTHING, new NothingArgs() { source = entity });
            }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isShouldAct && entity.hasAIStrategy && entity.aIStrategy.value == AIStrategyEnum.JUMPS;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ShouldAct);
    }
}