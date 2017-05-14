using Entitas;

public sealed class ProcGenFeature : Feature
{

    public ProcGenFeature(Contexts contexts) : base("ProcGen Systems")
    {
        Add(new RectangularMapSystem(contexts));
    }
}