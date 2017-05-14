using Entitas;
using System;
using System.Collections;

public sealed class CoroutineComponent : IComponent
{
    public IEnumerator value;
    public System.Action<GameEntity> callback;
}
