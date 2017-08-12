using System;
using System.Collections.Generic;
using Assets.Sources.Helpers.Networking;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class RectangularMapSystem : IInitializeSystem
{
    GameContext context;
	private readonly ActionsContext actionsContext;
    GameEntity gameBoard;

    public RectangularMapSystem(Contexts contexts)
    {
        context = contexts.game;
        gameBoard = context.gameBoardEntity;
	    actionsContext = contexts.actions;
    }

    public void Initialize()
    {
        if (!gameBoard.hasRectangularMap)
            return;

	    if (NetworkController.Instance.IsMultiplayer)
	    {
			Random.InitState(NetworkController.Instance.Seed);
		}

		var tunnelWidth = Random.Range(5, 10);
        var tunnelHeight = 3;
        var width = Random.Range(15, Math.Min(gameBoard.rectangularMap.width, 25));
        var height = Random.Range(15, Math.Min(gameBoard.rectangularMap.height, 25));
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
                            entity = context.CreateFloor(pos, Prefabs.Floor);
                        } else
                        {
                            entity = context.CreateWall(pos, Prefabs.WallDark);
                        }
                    }
                    else
                    {
                        entity = context.CreateFloor(pos, Prefabs.Floor);
                    }

                    Map.Instance.AddEntity(entity, pos);

                    if ((i == 0 || i == width - 1) && (j == 0 || j == height - 1))
                    {
                        var torch = context.CreateTorch(pos);
                        Map.Instance.AddEntity(torch, pos);
                    }
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
                    entity = context.CreateWall(pos, Prefabs.WallDark);
                } else
                {
                    entity = context.CreateFloor(pos, Prefabs.Floor);
                }
               

                Map.Instance.AddEntity(entity, pos);
            }
        }

	    {
		    var networkEntity = NetworkController.Instance.NetworkEntity;
		    if (networkEntity != null) // TODO: ugly
		    {
			    var i = 0;
			    foreach (var player in NetworkController.Instance.NetworkEntity.Players)
			    {
				    var pos = new IntVector2(8 + i, Random.Range(8, 11));
				    var focus = player.Id == NetworkController.Instance.NetworkEntity.Player.Id;

				    var entity = context.CreatePlayer(pos, focus, player.Name);

				    if (focus)
				    {
					    context.SetCurrentPlayer(entity, player);
				    }
				    i++;
			    }
		    }
		    else
		    {
			    var pos = new IntVector2(Random.Range(8, 11), Random.Range(8, 11));
			    var player = new Player(1, "Player");

			    var entity = context.CreatePlayer(pos, true, player.Name);
			    context.SetCurrentPlayer(entity, player);
		    }
	    }

		if (!NetworkController.Instance.IsMultiplayer || NetworkController.Instance.IsServer)
		{
			for (int i = 0; i < 10; i++)
			{
				var pos = new IntVector2(Random.Range(0, 5), Random.Range(0, 5));

				actionsContext.SpawnMonster(MonsterType.NakedMan, pos);

			}

			actionsContext.SpawnItem(ItemName.IronAxe, new IntVector2(10, 10));
		}
    }
}