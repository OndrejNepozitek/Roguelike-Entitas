using System;
using System.Collections.Generic;
using Entitas;

public class Map
{
    private List<GameEntity>[][] tiles;
    private int rows;
    private int cols;

    public static Map Instance;

    public Map(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        InitMap();
    }

    private void InitMap()
    {
        tiles = new List<GameEntity>[rows][];

        for (int i = 0; i < rows; i++)
        {
            tiles[i] = new List<GameEntity>[cols];

            for (int j = 0; j < cols; j++)
            {
                tiles[i][j] = new List<GameEntity>();
            }
        }
    }

    public List<GameEntity> GetTile(int x, int y)
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

    public bool IsWalkable(int x, int y)
    { 
        var tile = GetTile(x, y);

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

    public void AddEntity(GameEntity entity)
    {
        var position = entity.position.value;
        GetTile((int)position.x, (int)position.y).Add(entity); // TODO: maybe better conversion
    }

    public void MoveEntity(GameEntity entity)
    {
        var position = entity.previousPosition.value;
        GetTile((int)position.x, (int)position.y).Remove(entity); // TODO: maybe better conversion

        AddEntity(entity);
    }
}