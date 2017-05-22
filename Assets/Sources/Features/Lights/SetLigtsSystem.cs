using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class SetLightsSystem : ReactiveSystem<GameEntity>
{
    GameContext context;
    IGroup<GameEntity> group;

    public SetLightsSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
        group = context.GetGroup(GameMatcher.Shadow);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            foreach (var le in group.GetEntities())
            {
                le.ReplaceShadow(30);
            }

            var pos = entity.position.value;
            var entitiesToChange = Map.Instance.GetRhombWithoutCorners(pos, entity.light.radius);

            foreach (var le in entitiesToChange)
            {
                var distance = IntVector2.MaxDistance(pos, le.position.value);
                if (distance == 0) distance = 1;
                le.ReplaceShadow(100 - distance * 10);
            }

            
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLight;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position.Added(), GameMatcher.Light.Added());
    }

    private Color GetColorFromShadow(int shadow)
    {
        float val = shadow / 100f;
        return new Color(val, val, val);
    }
}