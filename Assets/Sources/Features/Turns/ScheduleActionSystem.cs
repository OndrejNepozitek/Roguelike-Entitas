using System;
using System.Collections.Generic;
using Entitas;

public sealed class ScheduleActionSystem : IExecuteSystem
{
    GameContext context;
    EventQueueComponent queueComponent;
    IGroup<GameEntity> group;
    IGroup<GameEntity> actions;

    public ScheduleActionSystem(Contexts contexts)
    {
        context = contexts.game;
        queueComponent = context.eventQueue;
        group = context.GetGroup(GameMatcher.TurnBased);
        actions = context.GetGroup(GameMatcher.Action);
    }

    public void Execute()
    {
        if (actions.GetEntities().Length != 0)
            return;

        /*foreach (var e in group.GetEntities())
        {
            if (e.isShouldAct || e.isActionInProgress)
            {
                return;
            }
        }*/

        var entity = queueComponent.queue.Dequeue();

        if (entity != null)
        {
            entity.isShouldAct = true;
        }
    }
}