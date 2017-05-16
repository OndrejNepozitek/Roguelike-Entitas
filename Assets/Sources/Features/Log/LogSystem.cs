using System;
using Entitas;
using System.Collections.Generic;

public sealed class LogSystem : IInitializeSystem, ICleanupSystem
{
    GameContext context;
    GameEntity logEntity;
    IGroup<GameEntity> group;

    public LogSystem(Contexts contexts)
    {
        context = contexts.game;
        group = context.GetGroup(GameMatcher.Action);
    }

    public void Cleanup()
    {
        var queue = logEntity.log.queue;
        var entities = group.GetEntities();

        foreach (var entity in entities)
        {
            var action = entity.action;

            if (action.type == ActionType.ATTACK)
            {
                var args = (AttackArgs) action.eventArgs;
                var source = args.source;
                var target = args.target;
                var amount = args.amount;

                var message = target.name.value + " was attacked by " + source.name.value + " (" + amount + " damage)";

                if (queue.Count == logEntity.log.maxSize)
                {
                    queue.Dequeue();
                }

                queue.Enqueue(message);
            }
        }

        if (entities.Length != 0)
        {
            logEntity.ReplaceLog(queue, logEntity.log.maxSize); // TODO: reaaally ugly
        }
    }

    public void Initialize()
    {
        logEntity = context.CreateEntity();
        logEntity.AddLog(new Queue<string>(), 6);
    }
}