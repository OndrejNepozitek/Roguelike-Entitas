using Entitas;
using System;
using System.Collections;

public sealed class CoroutineComponent : IComponent
{
    public IEnumerator value;
}