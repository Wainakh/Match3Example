using UnityEngine;

namespace M3T
{
    public struct PredictionPatternData
    {
        public Vector2Int movedPosition;
        public Vector2Int movedDirection;

        public PredictionPatternData(Vector2Int movedPosition, Vector2Int movedDirection)
        {
            this.movedPosition = movedPosition;
            this.movedDirection = movedDirection;
        }
    }
}