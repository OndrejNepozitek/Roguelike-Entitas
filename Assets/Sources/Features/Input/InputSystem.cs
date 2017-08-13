using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class InputSystem : IExecuteSystem, ICleanupSystem, IInitializeSystem
{
	private readonly ActionsContext actionsContext;
	private readonly GameContext gameContext;
	private GameEntity player;

	private bool lastHorizontal;

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
		// TODO: player should not be able to move while the game is loading

	    if (Input.GetKeyDown(KeyCode.E))
	    {
		    actionsContext.PickAndEquip(player.position.value, player);
		    return;
	    }

		var horizontal = (int)Input.GetAxisRaw("Horizontal");
        var vertical = (int)Input.GetAxisRaw("Vertical");

	    if (horizontal != 0 && vertical != 0)
	    {
		    if (lastHorizontal)
		    {
			    horizontal = 0;
		    }
		    else
		    {
			    vertical = 0;
		    }
	    }

	    if (horizontal != 0)
	    {
		    lastHorizontal = true;
	    }

	    if (vertical != 0)
	    {
		    lastHorizontal = false;
	    }

		var direction = IntVector2.GetGridDirection(horizontal, vertical);
		//var direction = new IntVector2(horizontal, vertical);

        if (direction != IntVector2.Empty)
        {
	        actionsContext.BasicMove(player, player.position.value + direction);
        }
    }

	public void Initialize()
	{
		player = gameContext.currentPlayerEntity.currentPlayer.Entity; // TODO: does not reflect current player changes
	}
}