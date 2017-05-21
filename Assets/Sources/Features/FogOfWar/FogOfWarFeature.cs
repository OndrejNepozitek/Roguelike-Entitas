using Entitas;

public sealed class FogOfWarFeature : Feature
{
    public FogOfWarFeature(Contexts contexts) : base("FogOfWar Systems")
    {
        Add(new AddFogSystem(contexts));
        Add(new RevealSystem(contexts));
        Add(new RemoveFogSystem(contexts));
    }
}