using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AddShadowSystem : ReactiveSystem<GameEntity>
{
    public AddShadowSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.hasShadow)
            {
                entity.ReplaceShadow(30);
            } else
            {
                entity.AddShadow(30);
            }
            /*var spriteRenderer = entity.view.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.yellow;*/
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.View.Added());
    }
}