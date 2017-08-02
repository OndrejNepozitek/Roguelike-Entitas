using System;
using System.Collections.Generic;
using System.Collections;
using Entitas;
using UnityEngine;

public sealed class ShouldDieSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    GameContext context;
    IGroup<GameEntity> group;

    public ShouldDieSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
        group = context.GetGroup(GameMatcher.Dead);
    }

    public void Cleanup()
    {
        foreach (var entity in group.GetEntities())
        {
            if (entity.isDead)
            {
                entity.Destroy();
            }
        }
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.health.Value <= 0)
            {
                entity.isDead = true;
            }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Health);
    }
}