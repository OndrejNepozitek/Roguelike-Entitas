using System;
using Entitas;
using UnityEngine;

public sealed class InputSystem : IExecuteSystem, ICleanupSystem, IInitializeSystem
{
	private readonly ActionsContext actionsContext;
	private readonly GameContext gameContext;
	private GameEntity player;

    public InputSystem(Contexts contexts)
    {
	    actionsContext = contexts.actions;
	    gameContext = contexts.game;
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

	    if (Input.GetKeyDown(KeyCode.E))
	    {
		    actionsContext.PickAndEquip(player.position.value, player);
		    return;
	    }

		var horizontal = (int)Input.GetAxisRaw("Horizontal");
        var vertical = (int)Input.GetAxisRaw("Vertical");
        var direction = IntVector2.GetGridDirection(horizontal, vertical);

        if (direction != new IntVector2())
        {
	        actionsContext.BasicMove(player, player.position.value + direction);
        }
    }

	public void Initialize()
	{
		player = gameContext.currentPlayerEntity.currentPlayer.Entity; // TODO: does not reflect current player changes
	}
}