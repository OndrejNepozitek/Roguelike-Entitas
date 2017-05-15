using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class PlayerCentricCameraSystem : IExecuteSystem, IInitializeSystem
{
    GameContext context;
    GameEntity camera;
    GameEntity player;
    IntVector2 offset;

    public PlayerCentricCameraSystem(Contexts contexts)
    {
        context = contexts.game;
        player = context.playerEntity;
        camera = context.cameraEntity;
    }

    public void Initialize()
    {
        offset = new IntVector2();
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

    public void Execute()
    {
        UpdatePosition();
    }
}