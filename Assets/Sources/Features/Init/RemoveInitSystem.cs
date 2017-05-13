using System;
using System.Collections.Generic;
using Entitas;

class RemoveInitSystem : ReactiveSystem<GameEntity>
{
    GameContext context;
    IGroup<GameEntity> group;

    public RemoveInitSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
        group = context.GetGroup(GameMatcher.Init);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in group.GetEntities())
        {
            entity.isInit = false;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isInit;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Init);
    }
}