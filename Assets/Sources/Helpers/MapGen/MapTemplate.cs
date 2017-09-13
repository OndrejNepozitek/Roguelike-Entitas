namespace Assets.Sources.Helpers.MapGen
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Map;

	/// <summary>
	/// Class for procedural map generation.
	/// </summary>
	public class MapTemplate : Map<TileType>
	{
		private readonly Random random;

		public MapTemplate(int width, int height, Random random) : base(width, height)
		{
			this.random = random;

			Init();
		}

		private void Init()
		{
			foreach (var tile in GetTiles())
			{
				tile.Value = TileType.Floor;
			}
		}

		public void RandomizeBorders(int maxDepth, int iterations, float newHoleChance, bool allowSoloHoles)
		{
			for (var i = 0; i < iterations; i++)
			{
				var emptyTiles = GetBorderTiles(maxDepth).Where(x => x.Value == TileType.Nothing).ToList();

				if (emptyTiles.Count == 0)
				{
					GetBorderTiles().GetRandom(random).Value = TileType.Nothing;
					continue;
				}

				List<MapTile> tilesToChoseFrom;

				
				var newHole = random.NextDouble() < newHoleChance;
				// Create a new hole
				if (newHole)
				{
					tilesToChoseFrom = GetBorderTiles().Where(x => x.Value != TileType.Nothing).ToList();
				}
				// Expand an existing hole
				else
				{
					var depth = 1;

					if (random.NextDouble() > 0.85)
					{
						depth = maxDepth;
					}

					tilesToChoseFrom = emptyTiles
						.Select(x => GetAdjacent(x.Position).AsEnumerable())
						.SelectMany(x => x)
						.Distinct()
						.Where(x => IsOnBorder(x.Position, depth) && x.Value == TileType.Floor)
						.ToList();
				}

				if (tilesToChoseFrom.Count == 0)
				{
					break;
				}

				var tile = tilesToChoseFrom.GetRandom(random);
				tile.Value = TileType.Nothing;

				if (newHole && !allowSoloHoles)
				{
					var adjacentTiles = GetAdjacent(tile.Position).Where(x => IsOnBorder(x.Position) && x.Value == TileType.Floor);
					if (adjacentTiles.Any())
					{
						adjacentTiles.GetRandom(random).Value = TileType.Nothing;
					}
				}
			}
		}

		public void RemoveUnreachableTiles()
		{
			foreach (var tile in GetTilesWhere(x => x != TileType.Nothing))
			{
				if (GetAdjacent(tile.Position).Count(x => x.Value != TileType.Nothing) == 0)
				{
					tile.Value = TileType.Nothing;
				}
			}
		}

		public void AddLake()
		{
			var tile = GetTilesWhere(x => x == TileType.Floor).GetRandom(random);
			tile.Value = TileType.Water;

			if (random.NextDouble() > 0.5)
			{
				var adjTile = GetAdjacent(tile.Position).GetRandom(random);
				adjTile.Value = TileType.Water;
			}
		}

		public void AddWalls()
		{
			AddPadding(1);

			foreach (var tile in GetTilesWhere(x => x == TileType.Nothing))
			{
				var adjacentFloor = GetAdjacentAndDiagonal(tile.Position).Where(x => x.Value != TileType.Nothing && x.Value != TileType.Wall);
				if (adjacentFloor.Count() != 0)
				{
					tile.Value = TileType.Wall;
				}
			}
		}

		private void AddPadding(int width)
		{
			var oldTiles = Tiles;
			Width = Width + 2 * width;
			Height = Height + 2 * width;
			var offset = new IntVector2(width, width);

			Initialize();

			for (var i = 0; i < oldTiles.Length; i++)
			{
				for (var j = 0; j < oldTiles[0].Length; j++)
				{
					SetTileValue(new IntVector2(i, j) + offset, oldTiles[i][j].Value);
				}
			}
		}
	}
}
