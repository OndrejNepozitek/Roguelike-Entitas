using System;
using System.Collections.Generic;
using Entitas;

public sealed class RemoveFromQueue : ReactiveSystem<GameEntity>
{
    GameContext context;
    EventQueueComponent queueComponent;

    public RemoveFromQueue(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
        queueComponent = context.eventQueue;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            queueComponent.queue.Remove(entity);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.isTurnBased;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TurnBased.Removed());
    }
}