using System;
using System.Collections.Generic;
using System.Collections;
using Entitas;
using UnityEngine;

public sealed class SmoothMovementSystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public SmoothMovementSystem(Contexts contexts) : base (contexts.game)
    {
        context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.isSmoothMovementInProgress)
            {
                continue;
            }

            entity.AddCoroutine(SmoothMovement(entity));
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView && entity.hasPosition && entity.hasSmoothMovement;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.SmoothMovement);
    }

    static IEnumerator SmoothMovement(GameEntity entity)
    {
        var gameObject = entity.view.gameObject;
        var transform = gameObject.transform;
        var end = (Vector3) entity.smoothMovement.target;
        var inverseMoveTime = 1f / entity.smoothMovement.moveTime;
        var sqrRemainingDistance = (transform.position -  end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector2 newPostion = Vector2.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPostion;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

        entity.isSmoothMovementInProgress = false;
        entity.ReplacePosition(end);
    }
}