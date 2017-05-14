using Entitas;
using UnityEngine;

public sealed class AssetComponent : IComponent
{
    public string name;
}

public sealed class PositionComponent : IComponent
{
    public IntVector2 value;
}

public sealed class PreviousPositionComponent : IComponent
{
    public IntVector2 value;
}

public sealed class ViewComponent : IComponent
{
    public GameObject gameObject;
}