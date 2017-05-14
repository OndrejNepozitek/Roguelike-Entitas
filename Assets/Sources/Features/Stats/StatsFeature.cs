using Entitas;

public sealed class StatsFeature : Feature
{
    public StatsFeature(Contexts contexts) : base("Stats Systems")
    {
        Add(new ShouldDieSystem(contexts));
    }
}