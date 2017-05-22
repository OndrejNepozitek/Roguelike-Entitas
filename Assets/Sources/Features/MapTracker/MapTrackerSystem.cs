using System;
using System.Collections.Generic;
using Entitas;

public sealed class MapTrackerSystem : ReactiveSystem<GameEntity>
{
    public MapTrackerSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.isMapTracked = true;
            entity.OnComponentAdded += Entity_OnComponentAdded;
            entity.OnComponentReplaced += Entity_OnComponentReplaced;
            entity.OnComponentRemoved += Entity_OnComponentRemoved;
        }
    }

    private void Entity_OnComponentRemoved(IEntity entity, int index, IComponent component)
    {
        var position = component as PositionComponent;

        if (position == null)
            return;

        Map.Instance.RemoveEntity(entity as GameEntity, position.value);
    }

    private void Entity_OnComponentReplaced(IEntity entity, int index, IComponent previousComponent, IComponent newComponent)
    {
        var prevPos = previousComponent as PositionComponent;
        var newPos = newComponent as PositionComponent;

        if (prevPos == null)
            return;

        Map.Instance.RemoveEntity(entity as GameEntity, prevPos.value);
        Map.Instance.AddEntity(entity as GameEntity, newPos.value);
    }

    private void Entity_OnComponentAdded(IEntity entity, int index, IComponent component)
    {
        var position = component as PositionComponent;

        if (position == null)
            return;

        Map.Instance.AddEntity(entity as GameEntity, position.value);
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.isMapTracked;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position);
    }
}