using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AIRandomMovesSystem : ReactiveSystem<GameEntity>
{
    GameContext context;

    public AIRandomMovesSystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (entities.Count > 1)
            throw new InvalidOperationException();

        foreach (var entity in entities)
        {
            entity.isShouldAct = false;
            entity.isActionInProgress = true;

            var pos = entity.position.value;
            List<Vector2> moves = new List<Vector2>();
            if (Map.Instance.IsWalkable((int)pos.x + 1, (int)pos.y))
                moves.Add(new Vector2((int)pos.x + 1, (int)pos.y));

            if (Map.Instance.IsWalkable((int)pos.x, (int)pos.y+1))
                moves.Add(new Vector2((int)pos.x, (int)pos.y+1));

            if (Map.Instance.IsWalkable((int)pos.x - 1, (int)pos.y))
                moves.Add(new Vector2((int)pos.x - 1, (int)pos.y));

            if (Map.Instance.IsWalkable((int)pos.x, (int)pos.y - 1))
                moves.Add(new Vector2((int)pos.x, (int)pos.y - 1));

            if (moves.Count == 0)
            {
                entity.isActionInProgress = false;
                context.CreateEntity().AddAction(ActionType.NOTHING, new NothingArgs() { source = entity });
                UnityEngine.Debug.Log("Nothing");
            } else
            {
                var move = moves[UnityEngine.Random.Range(0, moves.Count)];
                entity.ReplaceSmoothMovement(move, 0.5f);
            }

            
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isShouldAct && entity.hasAIStrategy && entity.aIStrategy.value == AIStrategyEnum.RANDOM_MOVES;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ShouldAct);
    }
}