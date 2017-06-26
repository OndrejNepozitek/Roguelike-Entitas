using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using System.Collections;

public sealed class SetPositionSystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public SetPositionSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var pos = entity.position.value;

            if (entity.position.smooth)
            {
                entity.AddCoroutine(SmoothMovement(entity), null);
            } else
            {
                entity.view.gameObject.transform.position = (Vector2)pos;
            }
            
            // TODO: what entities should create actions?
            if (entity.isTurnBased && !entity.isInit)
            {
                var action = context.CreateEntity();
                var eventArgs = new MoveArgs() { source = entity };
                action.AddAction(ActionType.MOVE, eventArgs);
            }
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView && entity.hasPosition;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position);
    }

    static IEnumerator SmoothMovement(GameEntity entity)
    {
        var gameObject = entity.view.gameObject;
        var transform = gameObject.transform;
        var end = entity.position.value;
        var endVector3 = (Vector3)((Vector2)end);
        var inverseMoveTime = 1f / 0.2f;
        var sqrRemainingDistance = (transform.position - endVector3).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector2 newPostion = Vector2.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPostion;
            sqrRemainingDistance = (transform.position - endVector3).sqrMagnitude;
            yield return null;
        }

        entity.isActionInProgress = false;
        entity.view.gameObject.transform.position = (Vector2)end;
    }
}