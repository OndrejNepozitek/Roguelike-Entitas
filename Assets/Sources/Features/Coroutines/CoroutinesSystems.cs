using Entitas;

public sealed class CoroutinesSystems : Feature
{

    public CoroutinesSystems(Contexts contexts) : base("Coroutines Systems")
    {
        Add(new CoroutineSystem(contexts));
    }
}