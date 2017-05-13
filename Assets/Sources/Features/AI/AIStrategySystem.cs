using System;
using System.Collections.Generic;
using Entitas;

public sealed class AIStrategySystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public AIStrategySystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (entities.Count > 1)
            throw new InvalidOperationException();

        foreach (var entity in entities)
        {

        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAIStrategy && entity.isOnTheMove;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.OnTheMove);
    }
}