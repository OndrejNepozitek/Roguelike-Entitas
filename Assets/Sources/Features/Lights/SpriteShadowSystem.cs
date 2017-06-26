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
            Color color;

            if (entity.hasInLight)
            {
                color = GetColorForLight(entity.inLight.value);
            } else
            {
                color = GetColorForShadow(entity.shadow.value);
            }

            var spriteRenderer = entity.view.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
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

    private static Color GetColorForShadow(int shadow)
    {
        var val = shadow / 100f;
        return new Color(val, val, val);
    }

    private static Color GetColorForLight(int light)
    {
        return new Color(light / 100f, light / 108f, light / 118f);
    }
}