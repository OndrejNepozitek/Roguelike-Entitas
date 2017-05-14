using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Unique]
public sealed class CameraComponent : IComponent
{
    public Camera value;
}