namespace Assets.Scripts
{
	using Sources.Helpers.Entitas;
	using Sources.Helpers.MapGen;
	using UnityEngine;
	using Random = System.Random;

	public class MapGeneration : MonoBehaviour
	{
		public int MinHeight;
		public int MaxHeight;

		public int MinWidth;
		public int MaxWidth;

		public bool WithWalls;
		public int CurrentSeed;
		public bool DoNotChangeSeed;

		public int BorderMaxDepth;
		public int BorderIterations;
		public float BorderNewHoleChance;
		public bool BorderAllowSoloHoles;

		public bool WithPools;
		public float PoolChance;

		public GameObject PrimaryMaterial;
		public GameObject SecondaryMaterial;
		public GameObject WallsMaterial;
		public GameObject WaterMaterial;

		private GameObject parentObject;
		private readonly Random seedGenearator = new Random();
		private Random random;

		private int width;
		private int height;
		private MapTemplate map;

		private void Awake()
		{
			parentObject = new GameObject("RootGameObject");
		}

		private void Start()
		{
			Generate();
		}

		public void Generate()
		{
			DeleteAll();

			if (!DoNotChangeSeed)
			{
				CurrentSeed = seedGenearator.Next(0, int.MaxValue);
			}

			random = new Random(CurrentSeed);

			width = random.Next(MinWidth, MaxWidth);
			height = random.Next(MinHeight, MaxHeight);

			map = new MapTemplate(width, height, random);
			map.RandomizeBorders(BorderMaxDepth, BorderIterations, BorderNewHoleChance, BorderAllowSoloHoles);
			map.RemoveUnreachableTiles();
			if (WithPools && random.NextDouble() < PoolChance) map.AddLake();
			if (WithWalls) map.AddWalls();
			SpawnMap();
		}

		private void SpawnMap()
		{
			foreach (var tile in map.GetTiles())
			{
				GameObject prefab = null;

				switch (tile.Value)
				{
					case TileType.Floor:
						prefab = PrimaryMaterial;
						break;

					case TileType.Wall:
						prefab = WallsMaterial;
						break;

					case TileType.Water:
						prefab = WaterMaterial;
						break;
				}

				if (prefab == null)
				{
					continue;
				}

				var go = Instantiate(prefab);

				go.transform.SetParent(parentObject.transform, false);
				go.transform.position = new Vector3(tile.Position.X, tile.Position.Y);
			}
		}

		public void DeleteAll()
		{
			foreach (var o in parentObject.transform.GetComponentsInChildren<Transform>())
			{
				if (o.gameObject != parentObject)
				{
					Destroy(o.gameObject);
				}
			}
		}

		private enum Tile
		{
			Nothing, Floor, Wall
		}
	}
}
