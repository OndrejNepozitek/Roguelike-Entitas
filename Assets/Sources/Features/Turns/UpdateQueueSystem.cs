using System;
using System.Collections.Generic;
using Entitas;

public sealed class UpdateQueueSystem : ReactiveSystem<GameEntity>
{
    GameContext context;
    EventQueueComponent queueComponent;

    public UpdateQueueSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
        queueComponent = context.eventQueue;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            // TODO: handle special cases
            queueComponent.queue.Enqueue(entity, entity.energy.energyCost);
            entity.energy.energyCost = 0;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isTurnBased && !entity.isInit;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Energy);
    }
}