using System;
using Entitas;

public sealed class ActionSystem : ICleanupSystem
{
    GameContext context;
    IGroup<GameEntity> group;

    public ActionSystem(Contexts contexts)
    {
        context = contexts.game;
        group = context.GetGroup(GameMatcher.Action);
    }

    public void Cleanup()
    {
        foreach (var entity in group.GetEntities())
        {
            entity.Destroy();
        }
    }
}