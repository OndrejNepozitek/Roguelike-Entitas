namespace Assets.Sources.Helpers.Map
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Graphs;

	public class EntityMap : IWeightedGraph<IntVector2>
	{
		private readonly int rows;
		private readonly int cols;
		private Map<List<GameEntity>> tiles;

		public EntityMap(int rows, int cols)
		{
			this.rows = rows;
			this.cols = cols;

			Initialize();
		}

		private void Initialize()
		{
			tiles = new Map<List<GameEntity>>(cols, rows);

			foreach (var tile in tiles.GetTiles())
			{
				tile.Value = new List<GameEntity>();
			}
		}

		public IList<GameEntity> GetEntitiesOnTile(IntVector2 pos)
		{
			return tiles.GetTileValue(pos);
		}

		public void AddEntity(GameEntity entity, IntVector2 pos)
		{
			var tile = tiles.GetTile(pos);

			if (!tile.Value.Contains(entity))
			{
				tile.Value.Add(entity);
			}
		}

		public void RemoveEntity(GameEntity entity, IntVector2 pos)
		{
			tiles.GetTile(pos).Value.Remove(entity);
		}

		public GameEntity TileHasAny(IntVector2 pos, Predicate<GameEntity> condition)
		{
			foreach (var entity in GetEntitiesOnTile(pos))
			{
				if (condition(entity))
				{
					return entity;
				}
			}

			return null;
		}

		public bool TileHasAll(IntVector2 pos, Predicate<GameEntity> condition)
		{
			foreach (var entity in GetEntitiesOnTile(pos))
			{
				if (!condition(entity))
				{
					return false;
				}
			}

			return true;
		}

		public bool IsWalkable(IntVector2 pos)
		{
			return TileHasAll(pos, e => !e.isSolid) && TileHasAny(pos, e => e.isFloor) != null;
		}

		public bool IsWalkable(int x, int y)
		{
			return TileHasAll(new IntVector2(x,y), e => !e.isSolid);
		}

		public bool IsInBorders(IntVector2 pos)
		{
			return pos.X >= 0 && pos.Y >= 0 && pos.X < cols && pos.Y < rows;
		}

		public GameEntity GetItem(IntVector2 pos)
		{
			return TileHasAny(pos, e => e.hasItem);
		}

		public bool ShouldLightSpread(IntVector2 pos)
		{
			return TileHasAll(pos, e => !e.isWall);
		}

		public IList<GameEntity> GetRhombWithoutCorners(IntVector2 center, int radius)
		{
			var tiles = new List<Map<List<GameEntity>>.MapTile>();
			var queue = new Queue<Map<List<GameEntity>>.MapTile>();
			var centerTile = this.tiles.GetTile(center);
			queue.Enqueue(centerTile);

			while (queue.Count != 0)
			{
				var tile = queue.Dequeue();

				if (IntVector2.ManhattanDistance(center, tile.Position) > radius || Math.Abs(center.X - tile.Position.X) == radius || Math.Abs(center.Y - tile.Position.Y) == radius)
					continue;

				if (!tiles.Contains(tile))
				{
					tiles.Add(tile);

					if (ShouldLightSpread(tile.Position) || tile == centerTile)
					{
						foreach (var p in tile.Position.GetAdjacentTilesAndDiagonal())
						{
							if (!this.tiles.IsInBorders(p)) continue;

							var toExplore = this.tiles.GetTile(p);
							if (toExplore != null)
							{
								queue.Enqueue(toExplore);
							}
						}
					}
				}
			}

			return GetEntitiesFromTiles(tiles);
		}

		private IList<GameEntity> GetEntitiesFromTiles(IEnumerable<Map<List<GameEntity>>.MapTile> tiles)
		{
			var entities = new List<GameEntity>();
			foreach (var tile in tiles)
			{
				entities.AddRange(tile.Value);
			}
			return entities;
		}

		/*public IEnumerable<GameEntity> GetSquareAround(IntVector2 center, int radius)
    {
        for (int i = 0; i < 2 * radius + 1; i++)
        {
            for (int j = 0; j < 2 * radius + 1; j++)
            {
                var pos = new IntVector2(center.x - radius + i, center.y - radius + j);

                if (IntVector2.ManhattanDistance(center, pos) > radius || Math.Abs(center.x - pos.x) == radius || Math.Abs(center.y - pos.y) == radius)
                {
                    continue;
                }

                var tile = GetTile(pos);

                if (tile != null)
                {
                    foreach (var entity in tile)
                    {
                        yield return entity;
                    }
                }
            }
        }
    }*/

		double IWeightedGraph<IntVector2>.Cost(IntVector2 node1, IntVector2 node2)
		{
			// TODO: it could later take into account worse tiles like water
			return 1;
		}

		IEnumerable<IntVector2> IWeightedGraph<IntVector2>.GetNeigbours(IntVector2 node)
		{
			var adjacent = node.GetAdjacentTiles();
			return adjacent.Where(x => tiles.IsInBorders(x) && IsWalkable(x));
		}
	}
}