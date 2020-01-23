using System.Collections.Generic;
using UnityEngine;

namespace M3T
{
    public static class BaseValues
    {
        public static Vector2Int[] directions = 
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right
        };

        public static Dictionary<TileType, Color> ColorsByType = new Dictionary<TileType, Color>()
        {
            {TileType.red, Color.red },
            {TileType.yellow, Color.yellow },
            {TileType.green, Color.green },
            {TileType.blue, Color.blue },
            {TileType.black, Color.black },
            {TileType.orange, new Color(255f/255f,128f/255f,0f/255f,1) },
            {TileType.purple, new Color(147f/255f,112f/255f,219f/255f,1) },
            {TileType.pink, new Color(255f/255f,192f/255f,203f/255f,1) },
            {TileType.grey, Color.gray },
            {TileType.white, Color.white },
        };

        // Need if in future will added chips which would be able to matches with diffenet types. Example: bombs.
        public static bool IsCompareTileTypes(TileType tileType1, TileType tileType2)
        {
            if (tileType1 == TileType.NONE || tileType2 == TileType.NONE)
                return false;
            return tileType1 == tileType2;
        }
    }
}