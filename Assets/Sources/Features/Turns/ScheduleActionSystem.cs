using System;
using System.Collections.Generic;
using Entitas;

public sealed class ScheduleActionSystem : IExecuteSystem
{
    GameContext context;
    EventQueueComponent queueComponent;
    IGroup<GameEntity> group;

    public ScheduleActionSystem(Contexts contexts)
    {
        context = contexts.game;
        queueComponent = context.eventQueue;
        group = context.GetGroup(GameMatcher.TurnBased);
    }

    public void Execute()
    {
        foreach (var e in group.GetEntities())
        {
            if (e.isShouldAct || e.isActionInProgress)
            {
                return;
            }
        }

        var entity = queueComponent.queue.Dequeue();
        if (entity != null)
        {
            entity.isShouldAct = true;
        }
    }
}