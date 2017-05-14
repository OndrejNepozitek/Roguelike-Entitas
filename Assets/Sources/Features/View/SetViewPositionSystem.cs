using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class SetViewPositionSystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public SetViewPositionSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var pos = entity.position.value;
            entity.view.gameObject.transform.position = (Vector2) pos;

            if (entity.hasAIStrategy && !entity.isInit)
            {
                var action = context.CreateEntity();
                var eventArgs = new MoveArgs() { source = entity };
                action.AddAction(ActionType.MOVE, eventArgs);
            }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView && entity.hasPosition;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position);
    }
}