
using UnityEngine;

namespace M3T
{
    public struct PredictionPattern : IMatchingPattern
    {
        public int weight { get; private set; }
        public Vector2Int[] vectors { get; private set; }
        public PredictionPatternData movedTile { get; private set; }

        public PredictionPattern(Vector2Int[] vectors)
        {
            this.weight = 0;
            this.vectors = vectors;
            this.movedTile = default;
        }

        public PredictionPattern(int weight, Vector2Int[] vectors)
        {
            this.weight = weight;
            this.vectors = vectors;
            this.movedTile = default;
        }

        public PredictionPattern(Vector2Int[] vectors, PredictionPatternData movedTile)
        {
            this.weight = 0;
            this.vectors = vectors;
            this.movedTile = movedTile;
        }

        public PredictionPattern(int weight, Vector2Int[] vectors, PredictionPatternData movedTile)
        {
            this.weight = weight;
            this.vectors = vectors;
            this.movedTile = movedTile;
        }
    }
}