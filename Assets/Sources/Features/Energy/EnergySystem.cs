using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class EnergySystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public EnergySystem(Contexts contexts) : base (contexts.game)
    {
        context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            switch (entity.action.type)
            {
                case ActionType.MOVE:
                    HandleMove((MoveArgs)entity.action.eventArgs);
                    break;
                case ActionType.NOTHING:
                    HandleNothing((NothingArgs)entity.action.eventArgs);
                    break;
            }
        }
    }

    private void HandleMove(MoveArgs args)
    {
        var source = args.source;

        if (!source.hasEnergy)
        {
            source.AddEnergy(0);
        }

        source.ReplaceEnergy(source.stats.movementSpeed + source.energy.energyCost);
    }

    private void HandleNothing(NothingArgs args)
    {
        var source = args.source;

        if (!source.hasEnergy)
        {
            source.AddEnergy(0);
        }

        source.ReplaceEnergy(50 + source.energy.energyCost);
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Action);
    }
}