namespace Assets.Sources.Helpers.Map
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	public class Map<T>
	{
		protected MapTile[][] Tiles { get; set; }
		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public Map(int width, int height)
		{
			Width = width;
			Height = height;

			Initialize();
		}

		protected void Initialize()
		{
			Tiles = new MapTile[Width][];

			for (var i = 0; i < Width; i++)
			{
				Tiles[i] = new MapTile[Height];

				for (var j = 0; j < Height; j++)
				{
					Tiles[i][j] = new MapTile() { Position = new IntVector2(i, j)};
				}
			}
		}

		private MapTile this[IntVector2 position]
		{
			get
			{
				return Tiles[position.X][position.Y];
			}
			set
			{
				Tiles[position.X][position.Y] = value;
			}
		}

		public MapTile GetTile(IntVector2 position)
		{
			return this[position];
		}

		public T GetTileValue(IntVector2 position)
		{
			return this[position].Value;
		}

		public void SetTileValue(IntVector2 position, T value)
		{
			this[position].Value = value;
		}

		public ReadOnlyCollection<MapTile> GetCorners()
		{
			var returnTiles = new List<MapTile>
			{
				Tiles[0][0],
				Tiles[0][Height - 1],
				Tiles[Width - 1][0],
				Tiles[Width - 1][Height - 1]
			};

			return returnTiles.AsReadOnly();
		}

		public ReadOnlyCollection<MapTile> GetTiles()
		{
			var returnTiles = new List<MapTile>();

			for (var i = 0; i < Width; i++)
			{
				for (var j = 0; j < Height; j++)
				{
					returnTiles.Add(Tiles[i][j]);
				}
			}

			return returnTiles.AsReadOnly();
		}

		public ReadOnlyCollection<MapTile> GetTilesWhere(Predicate<T> predicate)
		{
			var returnTiles = new List<MapTile>();

			for (var i = 0; i < Width; i++)
			{
				for (var j = 0; j < Height; j++)
				{
					if (predicate(Tiles[i][j].Value))
					{
						returnTiles.Add(Tiles[i][j]);
					}
				}
			}

			return returnTiles.AsReadOnly();
		}

		public bool IsInBorders(IntVector2 position)
		{
			return position.X >= 0 && position.X <= Width - 1 && position.Y >= 0 && position.Y <= Height - 1;
		}

		public ReadOnlyCollection<MapTile> GetAdjacent(IntVector2 position)
		{
			return (
				from pos in position.GetAdjacentTiles()
				where IsInBorders(pos)
				select GetTile(pos)
			).ToList().AsReadOnly();
		}

		public ReadOnlyCollection<MapTile> GetAdjacentAndDiagonal(IntVector2 position)
		{
			return (
				from pos in position.GetAdjacentTilesAndDiagonal()
				where IsInBorders(pos)
				select GetTile(pos)
			).ToList().AsReadOnly();
		}

		public ReadOnlyCollection<MapTile> GetBorderTiles(int depth = 1)
		{
			return GetTiles().Where(x => IsOnBorder(x.Position, depth)).ToList().AsReadOnly();
		}

		public bool IsOnBorder(IntVector2 position, int depth = 1)
		{
			return position.X < depth || position.X >= Width - depth || position.Y < depth || position.Y >= Height - depth;
		}

		public class MapTile
		{
			public IntVector2 Position;
			public T Value;
		}
	}
}
