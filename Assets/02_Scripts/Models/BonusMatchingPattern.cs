using UnityEngine;

namespace M3T
{
    public struct BonusMatchingPattern : IMatchingPattern
    {
        public int weight { get; private set; }
        public Vector2Int[] vectors { get; private set; }
        public BonusMatchingPatternData matchResult { get; private set; }

        public BonusMatchingPattern(Vector2Int[] vectors)
        {
            this.weight = 0;
            this.vectors = vectors;
            matchResult = default;
        }

        public BonusMatchingPattern(int weight, Vector2Int[] vectors)
        {
            this.weight = weight;
            this.vectors = vectors;
            matchResult = default;
        }

        public BonusMatchingPattern(int weight, Vector2Int[] vectors, BonusMatchingPatternData matchResult)
        {
            this.weight = weight;
            this.vectors = vectors;
            this.matchResult = matchResult;
        }
    }
}