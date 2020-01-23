using System;
using System.Collections.Generic;
using UnityEngine;

namespace M3T
{
    public interface IBoard
    {
        TileGenerator Generator { get; }
        Tile[,] Tiles { get; }
        List<List<Tile>> TilesAsListOfLines { get; }
        event Action<Tile, Tile> onSelectedTwoChips;
        event Action onUserSelectChip;
        Vector2Int BoardSize { get; }
        TileType[] GetNearestTileTypes(Vector2Int position);
        void CreateBoard();
        Vector2 TileOffset { get; }
        void ResetAllSelectedTiles();

        #region Временный костыль ввиду отсутсвия UserInput
        void OnTileClick(Tile tile);
        #endregion
    }
}