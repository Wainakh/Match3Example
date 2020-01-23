using UnityEngine;

namespace M3T
{
    public interface IMatchingPattern
    {
        int weight { get; }
        Vector2Int[] vectors { get; }
    }
}