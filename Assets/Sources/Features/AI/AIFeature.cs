using Entitas;

public sealed class AIFeature : Feature
{
    public AIFeature(Contexts contexts) : base("AI Systems")
    {
        Add(new RandomAISystem(contexts));
        Add(new JumpAISystem(contexts));
        Add(new SheepAndWolfFeature(contexts));
    }
}