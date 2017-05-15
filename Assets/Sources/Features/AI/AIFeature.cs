using Entitas;

public sealed class AIFeature : Feature
{
    public AIFeature(Contexts contexts) : base("AI Systems")
    {
        Add(new AIRandomMovesSystem(contexts));
        Add(new AIJumpSystem(contexts));
        Add(new SheepAndWolfFeature(contexts));
    }
}