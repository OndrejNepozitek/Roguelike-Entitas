using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class SpriteShadowSystem : ReactiveSystem<GameEntity>
{
    public SpriteShadowSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var spriteRenderer = entity.view.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = GetColorFromShadow(entity.shadow.value);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Shadow);
    }

    private Color GetColorFromShadow(int shadow)
    {
        float val = shadow / 100f;
        return new Color(val, val, val);
    }
}