using System;
using System.Collections.Generic;
using Entitas;

public sealed class AddFogSystem : ReactiveSystem<GameEntity>
{
    public AddFogSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.view.gameObject.SetActive(false);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.isRevealed && entity.hasView; // Maybe apply only to specified entities?
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.View.Added(), GameMatcher.Revealed.Removed());
    }
}