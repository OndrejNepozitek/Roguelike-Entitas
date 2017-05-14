using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class PlayerCentricCameraSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    GameContext context;
    GameEntity camera;
    GameEntity player;
    IntVector2 offset;

    public PlayerCentricCameraSystem(Contexts contexts) : base(contexts.game)
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
        Vector3 pos = (Vector2)(player.position.value + offset);
        pos.z = -5;
        camera.camera.value.transform.position = pos;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        UpdatePosition();
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position); // TODO: maybe overkill
    }
}