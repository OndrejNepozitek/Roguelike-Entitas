using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using System.Linq;

public sealed class WolfAISystem : ReactiveSystem<GameEntity>
{
    GameContext context;
    IGroup<GameEntity> group;

    public WolfAISystem(Contexts contexts) : base(contexts.game)
    {
        context = contexts.game;
        group = context.GetGroup(GameMatcher.SheepAI);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (entities.Count > 1)
            throw new InvalidOperationException();

        foreach (var entity in entities)
        {
            entity.isShouldAct = false;
            entity.isActionInProgress = true;

            var currentPos = entity.position.value;
            var moves = currentPos.GetAdjacentTiles().Where(x => Map.Instance.IsWalkable(x));

            foreach (var pos in currentPos.GetAdjacentTiles())
            {
                var tile = Map.Instance.GetEntitiesOnTile(pos);
                if (tile == null)
                    continue;

                foreach (var sheep in tile)
                {
                    if (sheep.isSheepAI)
                    {
                        context.CreateEntity().AddAction(ActionType.ATTACK, new AttackArgs() { source = entity, target = sheep, amount = 150 });
                        entity.isActionInProgress = false;
                        return;
                    }
                }
            }

            if (moves.Count() != 0)
            {
                var best = moves.ElementAt(0);
                var distance = float.MaxValue;

                foreach (var sheep in group.GetEntities())
                {
                    foreach (var move in moves)
                    {
                        if (IntVector2.ManhattanDistance(move, sheep.position.value) < distance)
                        {
                            best = move;
                            distance = IntVector2.ManhattanDistance(move, sheep.position.value);
                        }
                    }
                }

                entity.ReplaceSmoothMovement(best, 0.3f);
            }
            else
            {
                entity.isActionInProgress = false;
                context.CreateEntity().AddAction(ActionType.NOTHING, new NothingArgs() { source = entity });
                UnityEngine.Debug.Log("Nothing");
            }

            
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isWolfAI && entity.isAI;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ShouldAct);
    }
}