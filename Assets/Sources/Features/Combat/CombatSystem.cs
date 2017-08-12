using System;
using System.Collections.Generic;
using Entitas;

public sealed class CombatSystem : ICleanupSystem
{
    /*GameContext context;
    IGroup<GameEntity> group;*/

    public CombatSystem(Contexts contexts)
    {
        /*context = contexts.game;
        group = context.GetGroup(GameMatcher.ActionOld);*/
    }

    public void Cleanup()
    {
        /*foreach (var entity in group.GetEntities())
        {
            if (entity.ActionOld.type != ActionType.ATTACK)
                continue;

            var args = (AttackArgs)entity.ActionOld.eventArgs;
            var source = args.source;
            var target = args.target;

            target.ReplaceHealth(target.health.value - (int)args.amount);
        }*/
    }
}