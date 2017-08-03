using Entitas;
using UnityEngine;

public sealed class AssetComponent : IComponent
{
    public string name;
}

public sealed class PositionComponent : IComponent
{
    public IntVector2 value;
    public bool smooth;
}

public sealed class ViewComponent : IComponent
{
    public GameObject gameObject;
}

public sealed class ShouldBeDestroyed : IComponent
{
	
}