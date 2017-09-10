namespace Assets.Sources.Features.ProcGen
{
	using System;
	using Helpers;
	using Helpers.Map;
	using Helpers.Monsters;
	using Helpers.Networking;
	using Entitas;
	using Extensions;
	using Helpers.Items;
	using Helpers.SystemDependencies.Attributes;
	using MapTracker;
	using Scripts;
	using Random = UnityEngine.Random;

	[DependsOn(typeof(MapTrackerSystem))]
	public sealed class RectangularMapSystem : IInitializeSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;
		private readonly GameEntity gameBoard;

		public RectangularMapSystem(Contexts contexts)
		{
			gameContext = contexts.game;
			gameBoard = gameContext.gameBoardEntity;
			actionsContext = contexts.actions;
		}

		public void Initialize()
		{
			if (!gameBoard.hasRectangularMap)
				return;

			var map = gameContext.GetService<EntityMap>();
				
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
								entity = gameContext.CreateFloor(pos, Prefabs.Floor);
							} else
							{
								entity = gameContext.CreateWall(pos, Prefabs.WallDark);
							}
						}
						else
						{
							entity = gameContext.CreateFloor(pos, Prefabs.Floor);
						}

						map.AddEntity(entity, pos);

						if ((i == 0 || i == width - 1) && (j == 0 || j == height - 1))
						{
							var torch = gameContext.CreateTorch(pos);
							map.AddEntity(torch, pos);
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
						entity = gameContext.CreateWall(pos, Prefabs.WallDark);
					} else
					{
						entity = gameContext.CreateFloor(pos, Prefabs.Floor);
					}


					map.AddEntity(entity, pos);
				}
			}

			{
				if (NetworkController.Instance.IsMultiplayer) // TODO: ugly
				{
					var i = 0;
					foreach (var player in NetworkController.Instance.NetworkEntity.Players)
					{
						var pos = new IntVector2(8 + i, Random.Range(8, 11));
						var focus = player.Id == NetworkController.Instance.NetworkEntity.Player.Id;

						var entity = gameContext.CreatePlayer(pos, player);
						var id = EntityDatabase.Instance.AddEntity(entity);
						var entityRef = new EntityReference(id);
						entity.ReplaceNetworkTracked(entityRef);

						if (focus)
						{
							gameContext.SetCurrentPlayer(entityRef, player);
							gameContext.SetCameraTarget(entityRef);
						}
						i++;
					}
				}
				else
				{
					var pos = new IntVector2(Random.Range(8, 11), Random.Range(8, 11));
					var player = new Player(1, "Player");

					var entity = gameContext.CreatePlayer(pos, player);
					var id = EntityDatabase.Instance.AddEntity(entity);
					var entityRef = new EntityReference(id);
					entity.ReplaceNetworkTracked(entityRef); // TODO: stupid? it is needed for input.. but does not feel right while in singleplayer
					gameContext.SetCurrentPlayer(entityRef, player);
					gameContext.SetCameraTarget(entityRef);
				}
			}

			if (!NetworkController.Instance.IsMultiplayer || NetworkController.Instance.IsServer)
			{
				for (int i = 0; i < 10; i++)
				{
					var pos = new IntVector2(Random.Range(0, 5), Random.Range(0, 5));
					actionsContext.SpawnMonster(MonsterType.MonsterGreen, pos);
				}

				actionsContext.SpawnItem(ItemName.IronAxe, new IntVector2(10, 12));
				actionsContext.SpawnItem(ItemName.Torch, new IntVector2(10, 11));
				actionsContext.SpawnItem(ItemName.RoundIronShield, new IntVector2(11, 11));
				actionsContext.SpawnItem(ItemName.SteelAxe, new IntVector2(12, 11));
				actionsContext.SpawnMonster(MonsterType.BasicChest, new IntVector2(1, height - 2));
			}
		}
	}
}