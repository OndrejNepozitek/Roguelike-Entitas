using Entitas;

public sealed class ProcGenSystems : Feature
{

    public ProcGenSystems(Contexts contexts) : base("ProcGen Systems")
    {
        Add(new MapGenerationSystem(contexts));
    }
}