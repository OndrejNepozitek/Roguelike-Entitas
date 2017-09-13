namespace Assets.Sources.Features.ProcGen.Systems
{
	using System.IO;
	using Entitas;
	using Extensions;
	using Helpers;
	using Helpers.Map;
	using Helpers.Monsters;
	using Helpers.Networking;
	using Scripts;
	using Random = UnityEngine.Random;

	public class FileMapSystem : IInitializeSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;
		private readonly GameEntity gameBoard;
		private EntityMap map;

		public FileMapSystem(Contexts contexts)
		{
			gameContext = contexts.game;
			gameBoard = gameContext.gameBoardEntity;
			actionsContext = contexts.actions;
		}

		public void Initialize()
		{
			if (!gameBoard.hasRectangularMap)
				return;

			map = gameContext.GetService<EntityMap>();

			if (NetworkController.Instance.IsMultiplayer)
			{
				Random.InitState(NetworkController.Instance.Seed);
			}

			LoadFile(@"Assets\Sources\Features\ProcGen\Files\map_First layer.csv");
			LoadFile(@"Assets\Sources\Features\ProcGen\Files\map_Objects.csv");

			{
				if (NetworkController.Instance.IsMultiplayer) // TODO: ugly
				{
					var i = 0;
					foreach (var player in NetworkController.Instance.NetworkEntity.Players)
					{
						var pos = new IntVector2(6 + 2*i, 3 + 2*i);
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
					var pos = new IntVector2(6, 3);
					var player = new Player(1, "Player");

					var entity = gameContext.CreatePlayer(pos, player);
					var id = EntityDatabase.Instance.AddEntity(entity);
					var entityRef = new EntityReference(id);
					entity.ReplaceNetworkTracked(entityRef); // TODO: stupid? it is needed for input.. but does not feel right while in singleplayer
					gameContext.SetCurrentPlayer(entityRef, player);
					gameContext.SetCameraTarget(entityRef);
				}
			}
		}

		private void LoadFile(string file)
		{
			var lines = File.ReadAllLines(file);

			var y = lines.Length - 1;
			foreach (var line in lines)
			{
				var stringTiles = line.Split(',');
				var x = 0;

				foreach (var stringTile in stringTiles)
				{
					var tile = int.Parse(stringTile);
					var pos = new IntVector2(x, y);
					var entity = GetEntityFromTileId(tile, pos);

					x++;
					if (entity == null)
					{
						continue;
					}

					map.AddEntity(entity, pos);
				}

				y--;
			}
		}

		private GameEntity GetEntityFromTileId(int id, IntVector2 pos)
		{
			switch (id)
			{
				case 0:
					return gameContext.CreateWall(pos, Prefabs.WallDark);

				case 63:
					return gameContext.CreateFloor(pos, Prefabs.Floor);

				case 409:
					return gameContext.CreateTorch(pos);

				case 1235:
					return gameContext.CreateFloor(pos, Prefabs.StartMat);

				case 541:
					if (!NetworkController.Instance.IsMultiplayer || NetworkController.Instance.IsServer)
						actionsContext.SpawnMonster(MonsterType.BasicChest, pos);
					break;

				case 598:
					if (!NetworkController.Instance.IsMultiplayer || NetworkController.Instance.IsServer)
						actionsContext.SpawnMonster(MonsterType.BasicChest, pos);
					break;

				case 517:
					if (!NetworkController.Instance.IsMultiplayer || NetworkController.Instance.IsServer)
						actionsContext.SpawnMonster(MonsterType.MonsterGreen, pos);
					break;

				case 1:
					return gameContext.CreateFloor(pos, Prefabs.Water);

				case 1398:
					return gameContext.CreateWall(pos, Prefabs.WaterRockBig);

				case 1399:
					return gameContext.CreateWall(pos, Prefabs.WaterRockSmall);

				case 1038:
					return gameContext.CreateFloor(pos, Prefabs.IronStairs);
			}

			return null;
		}
	}
}
