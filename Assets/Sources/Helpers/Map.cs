using System;
using System.Collections.Generic;
using Entitas;
using System.Collections.ObjectModel;

public class Map
{
    private MapTile[][] tiles;
    private int rows;
    private int cols;

    public static Map Instance;

    class MapTile
    {
        public IntVector2 pos;
        public List<GameEntity> entities = new List<GameEntity>();

        public MapTile(int x, int y)
        {
            pos = new IntVector2(x, y);
        }

        public IList<GameEntity> GetEntities()
        {
            return entities.AsReadOnly();
        }

        public bool IsWalkable()
        {
            var tile = GetEntities();

            if (tile == null)
                return false;

            foreach (var entity in tile)
            {
                if (entity.isSolid)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public Map(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        InitMap();
    }

    private void InitMap()
    {
        tiles = new MapTile[rows][];

        for (int i = 0; i < rows; i++)
        {
            tiles[i] = new MapTile[cols];

            for (int j = 0; j < cols; j++)
            {
                var tile = new MapTile(i,j);
                tiles[i][j] = tile;
            }
        }
    }

    public IList<GameEntity> GenEntitiesOnTile(int x, int y)
    {
        var tile = GetTile(x, y);
        if (tile != null)
        {
            return tile.GetEntities();
        } else
        {
            return new List<GameEntity>();
        }
    }

    public IList<GameEntity> GetEntitiesOnTile(IntVector2 pos)
    {
        return GenEntitiesOnTile(pos.x, pos.y);
    }

    private MapTile GetTile(int x, int y)
    {
        if (x >= 0 && x < rows && y >= 0 && y < cols)
        {
            return tiles[x][y];
        }
        else
        {
            return null;
        }
    }

    private MapTile GetTile(IntVector2 pos)
    {
        return GetTile(pos.x, pos.y);
    }

    public void AddEntity(GameEntity entity, IntVector2 pos)
    {
        var tile = GetTile(pos);

        if (!tile.entities.Contains(entity))
        {
            tile.entities.Add(entity);
        }
    }

    public void RemoveEntity(GameEntity entity, IntVector2 pos)
    {
        GetTile(pos).entities.Remove(entity);
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
        var tiles = new List<MapTile>();
        var queue = new Queue<MapTile>();
        var centerTile = GetTile(center);
        queue.Enqueue(centerTile);

        while (queue.Count != 0)
        {
            var tile = queue.Dequeue();

            if (IntVector2.ManhattanDistance(center, tile.pos) > radius || Math.Abs(center.x - tile.pos.x) == radius || Math.Abs(center.y - tile.pos.y) == radius)
                continue;

            if (!tiles.Contains(tile))
            {
                tiles.Add(tile);

                if (ShouldLightSpread(tile.pos) || tile == centerTile)
                {
                    foreach (var p in tile.pos.GetAdjacentTilesAndDiagonal())
                    {
                        var toExplore = GetTile(p);
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

    private IList<GameEntity> GetEntitiesFromTiles(List<MapTile> tiles)
    {
        var entities = new List<GameEntity>();
        foreach (var tile in tiles)
        {
            entities.AddRange(tile.GetEntities());
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
}