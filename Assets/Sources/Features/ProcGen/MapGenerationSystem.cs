using System;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class MapGenerationSystem : IInitializeSystem
{
    readonly GameContext _context;

    public MapGenerationSystem(Contexts contexts)
    {
        _context = contexts.game;
    }

    public void Initialize()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                var entity = _context.CreateEntity();
                entity.isMapTile = true;
                entity.isSolid = false;
                entity.isFloor = true;
                entity.AddAsset(Prefabs.FLOOR);
                entity.AddPosition(new IntVector2(i, j));
                entity.AddPreviousPosition(new IntVector2());
            }
        }

        for (int i = 0; i < 20; i++)
        {
            var pos = new IntVector2(Random.Range(0, 5), Random.Range(0, 5));

            if (!Map.Instance.IsWalkable(pos.x, pos.y))
            {
                continue;
            }

            var entity = _context.CreateEntity();
            entity.AddPosition(pos);
            entity.isTurnBased = true;
            entity.isInit = true;
            entity.isSolid = true;
            entity.AddAsset(Prefabs.BODY_WHITE.ToString());
            entity.AddSmoothMovement(pos, 0.5f);
            entity.AddStats(30, 100, 10, 70);
            entity.AddHealth(100);

            if (UnityEngine.Random.Range(0,10) >= 7)
            {
                entity.AddAIStrategy(AIStrategyEnum.JUMPS);
            } else
            {
                entity.AddAIStrategy(AIStrategyEnum.RANDOM_MOVES);
            }

            entity.AddPreviousPosition(new IntVector2());
            Map.Instance.AddEntity(entity);
        }
    }
}