using System;
using Entitas;
using UnityEngine;

public sealed class InputSystem : IExecuteSystem, ICleanupSystem
{
    GameContext gameContext;
    InputContext inputContext;

    public InputSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        inputContext = contexts.input;
    }

    public void Cleanup()
    {
        // TODO: Are we sure there are not going to be other entities in the InputContext?
        foreach (var entity in inputContext.GetEntities())
        {
            entity.Destroy();
        }
    }

    public void Execute()
    {
        // Handle moving
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");
        var direction = IntVector2.GetGridDirection(horizontal, vertical);

        if (direction != new IntVector2())
        {
            inputContext.CreateEntity().AddMoveInput(direction);
        }
    }
}