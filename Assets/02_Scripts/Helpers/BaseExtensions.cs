using M3T;
using System.Collections.Generic;
using UnityEngine;

public static class BaseExtensions
{
    /// <summary>
    /// Check object for class or struct and equals for null or default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNullOrDefault<T>(this T obj)
    {
        if (obj == null)
            return true;
        if (typeof(T).IsValueType)
            return EqualityComparer<T>.Default.Equals(obj, default(T));
        else
            return obj.Equals(null);
    }

    public static List<Tile> GetNearestTiles(this Tile[,] tiles, Vector2Int center)
    {
        var result = new List<Tile>();
        var size = new Vector2Int(tiles.GetLength(0), tiles.GetLength(1));
        if (!IsPointBelongsSite(center, new Rect(Vector2Int.zero, size)))
            result.Add(tiles[center.x, center.y]);
        for (int i = 0; i < BaseValues.directions.Length; i++)
        {
            var targetPosition = center + BaseValues.directions[i];
            if (!IsPointBelongsSite(targetPosition, new Rect(Vector2Int.zero, size)))
                continue;
            result.Add(tiles[targetPosition.x, targetPosition.y]);
        }
        return result;
    }

    public static bool IsPointBelongsSite(Vector2 point, Rect site)
    {
        if (point.x < site.xMin || point.x >= site.xMax)
            return false;
        if (point.y < site.yMin || point.y >= site.yMax)
            return false;
        return true;
    }

    public static T[] ToArray<T>(this ICollection<T> collection)
    {
        T[] result = new T[collection.Count];
        int i = 0;
        var en = collection.GetEnumerator();
        while (en.MoveNext())
            result[i++] = en.Current;
        return result;
    }

    public static TileType GetTileTypeSafe(this Tile tile)
    {
        if (tile.IsNullOrDefault() || tile.tileData.IsNullOrDefault())
            return TileType.NONE;
        return tile.tileData.tileType;
    }
}