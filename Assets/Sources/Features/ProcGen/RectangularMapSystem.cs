using System;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class RectangularMapSystem : IInitializeSystem
{
    GameContext context;
    GameEntity gameBoard;

    public RectangularMapSystem(Contexts contexts)
    {
        context = contexts.game;
        gameBoard = context.gameBoardEntity;
    }

    public void Initialize()
    {
        if (!gameBoard.hasRectangularMap)
            return;

        var width = gameBoard.rectangularMap.width;
        var height = gameBoard.rectangularMap.height;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var entity = context.CreateEntity();
                entity.isMapTile = true;
                entity.isSolid = false;
                entity.isFloor = true;
                entity.AddAsset(Prefabs.FLOOR);
                entity.AddPosition(new IntVector2(i, j));
                entity.AddPreviousPosition(new IntVector2());
            }
        }

        for (int i = 0; i < 5; i++)
        {
            var pos = new IntVector2(Random.Range(0, 5), Random.Range(0, 5));

            if (!Map.Instance.IsWalkable(pos.x, pos.y))
            {
                continue;
            }

            var entity = context.CreateEntity();
            entity.AddPosition(pos);
            entity.isTurnBased = true;
            entity.isInit = true;
            entity.isSolid = true;
            entity.AddAsset(Prefabs.BODY_WHITE.ToString());
            entity.AddSmoothMovement(pos, 0.5f);
            entity.AddStats(30, 100, 10, 70);
            entity.AddHealth(100);
            entity.isAI = true;

            entity.isSheepAI = true;

            entity.AddPreviousPosition(new IntVector2());
            Map.Instance.AddEntity(entity);
        }

        {
            var pos = new IntVector2(Random.Range(8, 12), Random.Range(8,12));
            var entity = context.CreateEntity();
            entity.AddPosition(pos);
            entity.isTurnBased = true;
            entity.isInit = true;
            entity.isSolid = true;
            entity.AddAsset(Prefabs.BODY_BROWN.ToString());
            entity.AddSmoothMovement(pos, 0.5f);
            entity.AddStats(30, 100, 10, 1);
            entity.AddHealth(100);
            entity.isWolfAI = true;
            entity.isAI = true;

            entity.AddPreviousPosition(new IntVector2());
            Map.Instance.AddEntity(entity);
        }

    }
}