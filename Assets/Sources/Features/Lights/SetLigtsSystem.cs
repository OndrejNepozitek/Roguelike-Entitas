using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class SetLightsSystem : ReactiveSystem<GameEntity>
{
    GameContext context;
    IGroup<GameEntity> inLightGroup;
    IGroup<GameEntity> isLightGroup;

    public SetLightsSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
        inLightGroup = context.GetGroup(GameMatcher.InLight);
        isLightGroup = context.GetGroup(GameMatcher.Light);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        // TODO: Should be optimized later
        foreach (var le in inLightGroup.GetEntities())
        {
            le.RemoveInLight();
        }

        foreach (var entity in isLightGroup.GetEntities())
        {
            if (entity.isRevealed)
            {
                if (entity.hasLight)
                {
                    EditNearbyLights(entity);
                } else
                {
                    var floor = Map.Instance.TileHasAny(entity.position.value, e => e.isFloor);
                    
                    if (entity.hasInLight)
                    {
                        entity.ReplaceInLight(floor.inLight.value);
                    } else
                    {
                        entity.AddLight(floor.inLight.value);
                    }
                }
            }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position.Added(), GameMatcher.Light.Added());
    }

    private void EditNearbyLights(GameEntity entity)
    {
        var pos = entity.position.value;
        var entitiesToChange = Map.Instance.GetRhombWithoutCorners(pos, entity.light.radius);

        foreach (var le in entitiesToChange)
        {
            var distance = IntVector2.MaxDistance(pos, le.position.value);
            if (distance == 0) distance = 1;
            var newVal = 100 - distance * 10;
            
            if (le.hasInLight)
            {
                if (le.inLight.value < newVal)
                {
                    le.inLight.value = newVal;
                }
            } else
            {
                le.AddInLight(newVal);
            }
        }
    }
}