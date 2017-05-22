using Entitas;

public sealed class LightsFeature : Feature
{
    public LightsFeature(Contexts contexts) : base("Lights Systems")
    {
        Add(new AddShadowSystem(contexts));
        Add(new SetLightsSystem(contexts));
        Add(new SpriteShadowSystem(contexts));
    }
}