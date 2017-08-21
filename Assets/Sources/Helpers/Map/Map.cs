using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Sources.Helpers.Map
{
	using System.Collections.ObjectModel;
	using Entitas;
	using UnityEngine;

	public class Map<T>
	{
		protected MapTile[][] tiles { get; set; }
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
			tiles = new MapTile[Width][];

			for (var i = 0; i < Width; i++)
			{
				tiles[i] = new MapTile[Height];

				for (var j = 0; j < Height; j++)
				{
					tiles[i][j] = new MapTile() { Position = new IntVector2(i, j)};
				}
			}
		}

		private MapTile this[IntVector2 position]
		{
			get
			{
				return tiles[position.X][position.Y];
			}
			set
			{
				tiles[position.X][position.Y] = value;
			}
		}

		public MapTile GeTile(IntVector2 position)
		{
			return this[position];
		}

		public T GeTileValue(IntVector2 position)
		{
			return this[position].Value;
		}

		public void SetTileValue(IntVector2 position, T value)
		{
			this[position].Value = value;
		}

		public ReadOnlyCollection<MapTile> GetCorners()
		{
			var returnTiles = new List<MapTile>();

			returnTiles.Add(tiles[0][0]);
			returnTiles.Add(tiles[0][Height - 1]);
			returnTiles.Add(tiles[Width - 1][0]);
			returnTiles.Add(tiles[Width - 1][Height - 1]);

			return returnTiles.AsReadOnly();
		}

		public ReadOnlyCollection<MapTile> GetTiles()
		{
			var returnTiles = new List<MapTile>();

			for (var i = 0; i < Width; i++)
			{
				for (var j = 0; j < Height; j++)
				{
					returnTiles.Add(tiles[i][j]);
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
					if (predicate(tiles[i][j].Value))
					{
						returnTiles.Add(tiles[i][j]);
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
				select GeTile(pos)
			).ToList().AsReadOnly();
		}

		public ReadOnlyCollection<MapTile> GetAdjacentAndDiagonal(IntVector2 position)
		{
			return (
				from pos in position.GetAdjacentTilesAndDiagonal()
				where IsInBorders(pos)
				select GeTile(pos)
			).ToList().AsReadOnly();
		}

		public ReadOnlyCollection<MapTile> GetBorderTiles(int depth = 1)
		{
			return GetTiles().Where(x => IsOnBorder(x.Position, depth)).ToList().AsReadOnly();
		}

		public bool IsOnBorder(IntVector2 position, int depth = 1)
		{
			return (position.X < depth || position.X >= Width - depth) || (position.Y < depth || position.Y >= Height - depth);
		}

		public class MapTile
		{
			public IntVector2 Position;
			public T Value;
		}
	}
}
