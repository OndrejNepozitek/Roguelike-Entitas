using Entitas;

public sealed class ActionOld : IComponent
{
    public ActionType type;
    public IEventArgs eventArgs;
}