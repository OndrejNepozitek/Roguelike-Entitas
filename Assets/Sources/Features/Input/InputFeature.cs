using Entitas;

public sealed class InputFeature : Feature
{
    public InputFeature(Contexts contexts) : base("Input Systems")
    {
        Add(new InputSystem(contexts));
    }
}