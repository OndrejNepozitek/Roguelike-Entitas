using Entitas;

public sealed class ViewSystems : Feature
{

    public ViewSystems(Contexts contexts) : base("View Systems")
    {
        Add(new AddViewSystem(contexts));
        Add(new SetViewPositionSystem(contexts));
        Add(new RemoveViewSystem(contexts));
        Add(new PreviousPositionSystem(contexts));
    }
}