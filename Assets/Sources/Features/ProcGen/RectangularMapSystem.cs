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
                var pos = new IntVector2(i, j);


                entity.AddPosition(pos);

                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    entity.AddAsset(Prefabs.WALL_DARK);
                    entity.isSolid = true;
                    entity.isWall = false;
                } else
                {
                    entity.AddAsset(Prefabs.FLOOR);
                }

                Map.Instance.AddEntity(entity, pos);
            }
        }

        for (int i = 0; i < 10; i++)
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
            entity.AddName("Good Sheep " + i);
            Map.Instance.AddEntity(entity, pos);
        }

        {
            var pos = new IntVector2(Random.Range(8, 12), Random.Range(8,12));
            //var entity = context.CreateEntity();
            
            var entity = context.playerEntity;
            entity.AddPosition(pos);
            entity.isTurnBased = true;
            entity.isInit = true;
            entity.isSolid = true;
            entity.AddAsset(Prefabs.BODY_BROWN.ToString());
            entity.AddSmoothMovement(pos, 0.05f);
            entity.AddStats(30, 100, 10, 1);
            entity.AddHealth(100);
            entity.isWolfAI = true;
            //entity.isAI = true;
            entity.AddName("Angry Wolf");
            entity.AddRevealAround(5);
            entity.AddLight(5);
            Map.Instance.AddEntity(entity, pos);
        }

    }
}