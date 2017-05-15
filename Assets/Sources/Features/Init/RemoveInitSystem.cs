using System;
using System.Collections.Generic;
using Entitas;

class RemoveInitSystem : ICleanupSystem
{
    GameContext context;
    IGroup<GameEntity> group;

    public RemoveInitSystem(Contexts contexts) 
    {
        context = contexts.game;
        group = context.GetGroup(GameMatcher.Init);
    }

    public void Cleanup()
    {
        foreach (var entity in group.GetEntities())
        {
            entity.isInit = false;
        }
    }
}