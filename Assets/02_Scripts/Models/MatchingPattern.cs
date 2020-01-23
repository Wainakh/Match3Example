using UnityEngine;

namespace M3T
{
    public struct MatchingPattern : IMatchingPattern
    {
        public int weight { get; private set; }
        public Vector2Int[] vectors { get; private set; }

        public MatchingPattern(Vector2Int[] vectors)
        {
            this.weight = 0;
            this.vectors = vectors;
        }

        public MatchingPattern(int weight, Vector2Int[] vectors)
        {
            this.weight = weight;
            this.vectors = vectors;
        }
    }
}