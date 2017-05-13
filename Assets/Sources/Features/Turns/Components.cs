using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class TurnBasedComponent : IComponent
{

}

[Unique]
public sealed class EventQueueComponent : IComponent
{
    public EventQueue<GameEntity> queue = new EventQueue<GameEntity>();
}