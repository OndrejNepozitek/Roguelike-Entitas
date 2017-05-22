using Entitas;

// The default value when not in light
public sealed class ShadowComponent : IComponent
{
    public int value;
}

// Actual light value if in light
public sealed class InLightComponent : IComponent
{
    public int value;
}

// When you produce light
public sealed class LightComponent : IComponent
{
    public int radius;
}