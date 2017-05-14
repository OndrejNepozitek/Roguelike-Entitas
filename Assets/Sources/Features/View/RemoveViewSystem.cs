using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public sealed class RemoveViewSystem : ReactiveSystem<GameEntity>
{
    readonly GameContext _context;

    public RemoveViewSystem(Contexts contexts) : base(contexts.game)
    {
        _context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            RemoveView(entity);
        }
    }

    private void RemoveView(GameEntity entity)
    {
        entity.view.gameObject.Unlink();
        UnityEngine.Object.Destroy(entity.view.gameObject);
        entity.RemoveView();
        
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return new Collector<GameEntity>(
            new[] {
                context.GetGroup(GameMatcher.Asset),
                context.GetGroup(GameMatcher.Destroyed),
                context.GetGroup(GameMatcher.Dead)
            },
            new[] {
                GroupEvent.Removed,
                GroupEvent.Added,
                GroupEvent.Added
            }
        );
    }
}