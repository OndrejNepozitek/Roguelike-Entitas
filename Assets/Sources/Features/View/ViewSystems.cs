using Entitas;

public sealed class ViewSystems : Feature
{

    public ViewSystems(Contexts contexts) : base("View Systems")
    {
        Add(new AddViewSystem(contexts));
        Add(new SetPositionSystem(contexts));
        Add(new PreviousPositionSystem(contexts));
    }
}