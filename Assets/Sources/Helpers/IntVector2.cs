﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct IntVector2
{
    public int x;
    public int y;

    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
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
            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();
            return hash;
        }
    }

    public List<IntVector2> GetAdjacentTiles()
    {
        List<IntVector2> positions = new List<IntVector2>();

        positions.Add(new IntVector2(x + 1, y));
        positions.Add(new IntVector2(x - 1, y));
        positions.Add(new IntVector2(x, y + 1));
        positions.Add(new IntVector2(x, y - 1));

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
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    public static double EuclideanDistance(IntVector2 a, IntVector2 b)
    {
        return Math.Sqrt((int)(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2)));
    }

    public static int MaxDistance(IntVector2 a, IntVector2 b)
    {
        return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
    }


    // OPERATORS
    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x + b.x, a.y + b.y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x - b.x, a.y - b.y);
    }

    public static IntVector2 operator *(int a, IntVector2 b)
    {
        return new IntVector2(a * b.x, a * b.y);
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        return a.x != b.x || a.y != b.y;
    }


    // CONVERSIONS
    public static implicit operator Vector2(IntVector2 a)
    {
        return new Vector2(a.x, a.y);
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
        return new Vector3(a.x, a.y, 0);
    }
}