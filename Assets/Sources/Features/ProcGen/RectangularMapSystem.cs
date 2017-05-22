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

        var tunnelWidth = Random.Range(5, 10);
        var tunnelHeight = 3;
        var width = Random.Range(12, Math.Min(gameBoard.rectangularMap.width, 20));
        var height = Random.Range(12, Math.Min(gameBoard.rectangularMap.height, 20));
        var tunnelPos = Random.Range(1, height - 1 - tunnelHeight);


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    var pos = new IntVector2(i, j);
                    if (k == 1)
                    {
                        pos += new IntVector2(width + tunnelWidth, 0);
                    }

                    GameEntity entity;

                    if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    {
                        if ((k == 0 && i == width - 1 && (j >= tunnelPos && j < tunnelPos + tunnelHeight)) || (k == 1 && i == 0 && (j >= tunnelPos && j < tunnelPos + tunnelHeight)))
                        {
                            entity = context.CreateFloor(pos, Prefabs.FLOOR);
                        } else
                        {
                            entity = context.CreateWall(pos, Prefabs.WALL_DARK);
                        }
                    }
                    else
                    {
                        entity = context.CreateFloor(pos, Prefabs.FLOOR);
                    }

                    Map.Instance.AddEntity(entity, pos);
                }


            }
        }

        for (int i = 0; i < tunnelWidth; i++)
        {
            for (int j = 0; j < tunnelHeight + 2; j++)
            {
                var pos = new IntVector2(i + width, tunnelPos + j - 1);
                GameEntity entity;

                if (j == 0 || j == tunnelHeight + 1)
                {
                    entity = context.CreateWall(pos, Prefabs.WALL_DARK);
                } else
                {
                    entity = context.CreateFloor(pos, Prefabs.FLOOR);
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
            var pos = new IntVector2(Random.Range(8, 11), Random.Range(8, 11));
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