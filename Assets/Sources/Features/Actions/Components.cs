using Entitas;

public sealed class Action : IComponent
{
    public ActionType type;
    public IEventArgs eventArgs;
}