using System;
using Entitas;
using UnityEngine;

public sealed class InputSystem : IExecuteSystem, ICleanupSystem
{
	private readonly ActionsContext actionsContext;
	private readonly GameEntity player;

    public InputSystem(Contexts contexts)
    {
	    actionsContext = contexts.actions;
	    var gameContext = contexts.game;
	    player = gameContext.playerEntity;
    }

    public void Cleanup()
    {
        // TODO: Are we sure there are not going to be other entities in the InputContext?
        /*foreach (var entity in inputContext.GetEntities())
        {
            entity.Destroy();
        }*/
    }

    public void Execute()
    {
        // Handle moving
        var horizontal = (int)Input.GetAxisRaw("Horizontal");
        var vertical = (int)Input.GetAxisRaw("Vertical");
        var direction = IntVector2.GetGridDirection(horizontal, vertical);

        if (direction != new IntVector2())
        {
	        actionsContext.CreateBasicMove(player, direction);
        }
    }
}