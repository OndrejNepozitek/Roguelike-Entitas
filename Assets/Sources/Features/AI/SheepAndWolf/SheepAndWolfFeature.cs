using Entitas;

public sealed class SheepAndWolfFeature : Feature
{
    public SheepAndWolfFeature(Contexts contexts) : base("SheepAndWolf Systems")
    {
        Add(new SheepAISystem(contexts));
        Add(new WolfAISystem(contexts));
    }
}