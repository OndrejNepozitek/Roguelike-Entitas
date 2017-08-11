using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class IntVector2
{
	[ProtoMember(1)]
    public int X;

	[ProtoMember(2)]
    public int Y;

	public IntVector2()
	{
		
	}

    public IntVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

	public override string ToString()
	{
		return string.Format("IntVector2 ({0}, {1})", X, Y);
	}

	public override bool Equals(object obj)
    {
        if (!(obj is IntVector2))
            return false;

        var vector = (IntVector2)obj;
        return vector == this;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }
    }

    public List<IntVector2> GetAdjacentTiles()
    {
        List<IntVector2> positions = new List<IntVector2>();

        positions.Add(new IntVector2(X + 1, Y));
        positions.Add(new IntVector2(X - 1, Y));
        positions.Add(new IntVector2(X, Y + 1));
        positions.Add(new IntVector2(X, Y - 1));

        return positions;
    }

    public List<IntVector2> GetAdjacentTilesAndDiagonal()
    {
        List<IntVector2> positions = GetAdjacentTiles();

        positions.Add(new IntVector2(X + 1, Y + 1));
        positions.Add(new IntVector2(X - 1, Y - 1));
        positions.Add(new IntVector2(X - 1, Y + 1));
        positions.Add(new IntVector2(X + 1, Y - 1));

        return positions;
    }

    public static IntVector2 GetGridDirection(int x, int y)
    {
        if (x != 0)
            y = 0;

        if (y != 0)
            x = 0;

        return new IntVector2(x, y);
    }

    public static int ManhattanDistance(IntVector2 a, IntVector2 b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    public static double EuclideanDistance(IntVector2 a, IntVector2 b)
    {
        return Math.Sqrt((int)(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2)));
    }

    public static int MaxDistance(IntVector2 a, IntVector2 b)
    {
        return Math.Max(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
    }


    // OPERATORS
    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X + b.X, a.Y + b.Y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X - b.X, a.Y - b.Y);
    }

    public static IntVector2 operator *(int a, IntVector2 b)
    {
        return new IntVector2(a * b.X, a * b.Y);
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        return a.X != b.X || a.Y != b.Y;
    }


    // CONVERSIONS
    public static implicit operator Vector2(IntVector2 a)
    {
        return new Vector2(a.X, a.Y);
    }

    public static explicit operator IntVector2(Vector2 a)
    {
        return new IntVector2((int) a.x, (int) a.y);
    }

    public static explicit operator IntVector2(Vector3 a)
    {
        return new IntVector2((int) a.x, (int) a.y);
    }

    public static explicit operator Vector3(IntVector2 a)
    {
        return new Vector3(a.X, a.Y, 0);
    }
}