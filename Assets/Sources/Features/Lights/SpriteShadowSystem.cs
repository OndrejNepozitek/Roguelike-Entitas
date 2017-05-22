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
            int lightValue;

            if (entity.hasInLight)
            {
                lightValue = entity.inLight.value;
            } else
            {
                lightValue = entity.shadow.value;
            }

            var spriteRenderer = entity.view.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = GetColorFromShadow(lightValue);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.InLight.AddedOrRemoved());
    }

    private Color GetColorFromShadow(int shadow)
    {
        float val = shadow / 100f;
        return new Color(val, val, val);
    }
}