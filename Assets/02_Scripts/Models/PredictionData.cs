using UnityEngine;

namespace M3T
{
    /// <summary>
    /// Список фишек, которые нужно подсветить
    /// Фишку, которую нужно анимировать движением и вектор, в какую сторону
    /// </summary>
    public struct PredictionData
    {
        public Tile[] tiles;
        public Tile target;
        public Vector2 direction;
    }
}