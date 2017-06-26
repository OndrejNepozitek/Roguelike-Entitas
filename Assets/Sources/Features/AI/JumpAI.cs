using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class JumpAISystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public JumpAISystem(Contexts contexts) : base(contexts.game)
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
                var pos = currentPos + new IntVector2(3 * UnityEngine.Random.Range(-1, 2), 3 * UnityEngine.Random.Range(-1, 2));
                if (Map.Instance.IsWalkable(pos))
                {
                    entity.ReplacePosition(pos, true);
                    moved = true;
                    break;
                }
            }

            if (moved == false)
            {
                entity.isActionInProgress = false;
                context.CreateEntity().AddAction(ActionType.NOTHING, new NothingArgs() { source = entity });
            }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isJumpAI;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ShouldAct);
    }
}