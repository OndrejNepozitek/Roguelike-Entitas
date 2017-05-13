using Entitas;
using UnityEngine;

public sealed class SmoothMovementComponent : IComponent
{
    public Vector2 target;
    public float moveTime;
}