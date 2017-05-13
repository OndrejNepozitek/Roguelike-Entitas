using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class PreviousPositionSystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public PreviousPositionSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (!entity.hasPreviousPosition)
            {
                entity.AddPreviousPosition(new Vector2());
                Map.Instance.AddEntity(entity);
                continue;
            }

            if (entity.position.value == entity.previousPosition.value)
                continue;

            Map.Instance.MoveEntity(entity);
            entity.previousPosition.value = entity.position.value;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPosition;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.PreviousPosition);
    }
}