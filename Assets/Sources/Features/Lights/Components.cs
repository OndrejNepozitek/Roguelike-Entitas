using Entitas;

public sealed class ShadowComponent : IComponent
{
    public int value;
}

public sealed class LightComponent : IComponent
{
    public int radius;
}