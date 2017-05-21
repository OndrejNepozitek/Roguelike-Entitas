using System;
using System.Collections.Generic;
using Entitas;

public sealed class RevealSystem : ReactiveSystem<GameEntity>
{
    public RevealSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            foreach (var revealEntity in Map.Instance.GetRhombWithoutCorners(entity.position.value, entity.revealAround.radius))
            {
                if (!revealEntity.isRevealed)
                {
                    revealEntity.isRevealed = true;
                }
            }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasRevealAround;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position.Added());
    }
}