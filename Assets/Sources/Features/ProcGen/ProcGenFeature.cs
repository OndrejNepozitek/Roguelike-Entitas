public sealed class ProcGenFeature : Assets.Sources.Helpers.SystemDependencies.Feature
{
    public ProcGenFeature(Contexts contexts) : base("ProcGen Systems")
    {
        Add(new RectangularMapSystem(contexts));
    }
}