using UnityEngine;

namespace M3T
{
    public struct BonusMatchingPatternData
    {
        public Vector2Int position;
        public TileData tileData;

        public BonusMatchingPatternData(Vector2Int position, TileData tileData)
        {
            this.position = position;
            this.tileData = tileData;
        }
    }
}