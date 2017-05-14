using Entitas;
using UnityEngine;

public sealed class SmoothMovementComponent : IComponent
{
    public IntVector2 target;
    public float moveTime;
}