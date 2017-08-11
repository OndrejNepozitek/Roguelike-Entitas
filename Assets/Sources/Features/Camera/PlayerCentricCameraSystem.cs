using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using System.Linq;

public sealed class PlayerCentricCameraSystem : ICleanupSystem, IInitializeSystem
{
    GameContext context;
    GameEntity camera;
    GameEntity player;
    IntVector2 offset;
	private IGroup<GameEntity> players;

    public PlayerCentricCameraSystem(Contexts contexts)
    {
	    context = contexts.game;
        camera = context.cameraEntity;
    }

    public void Initialize()
    {
        offset = new IntVector2();
	    player = context.currentPlayerEntity.currentPlayer.Entity; // TODO: does not reflect current player changes
		UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 pos;

        if (player.hasView)
        {
            pos = (player.view.gameObject.transform.position + (Vector3)offset);
        } else
        {
            pos = (Vector3)(player.position.value + offset);
        }

        pos.z = -5;
        camera.camera.value.transform.position = pos;
       
    }

	public void Cleanup()
	{
		UpdatePosition();
	}
}