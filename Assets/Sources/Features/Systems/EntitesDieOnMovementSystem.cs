using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class EntitiesDieOnMovementSystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public EntitiesDieOnMovementSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        /*foreach (var entity in entities)
        {
            var args = (MoveArgs)entity.ActionOld.eventArgs;
            var source = args.source;

            if (source.hasHealth)
            {
                source.ReplaceHealth(source.health.value - Random.Range(0, 60));
            }
            
        }*/
    }

    protected override bool Filter(GameEntity entity)
    {
	    return true; //entity.ActionOld.type == ActionType.MOVE;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ActionOld);
    }
}