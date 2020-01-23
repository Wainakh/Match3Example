using System;
using System.Collections.Generic;
using UnityEngine;

namespace M3T
{
    /// <summary>
    /// Доска, которая хранит фишки на поле
    /// </summary>
    public class Board : MonoBehaviour, IBoard
    {
        public TileGenerator Generator { get; private set; }
        public Tile[,] Tiles { get; private set; }
        public Vector2 TileOffset { get; private set; }

        public event Action<Tile, Tile> onSelectedTwoChips;
        public event Action onUserSelectChip;

        [SerializeField] Vector2Int boardSize;
        public Vector2Int BoardSize => boardSize;

        Transform boardRoot;
        Tile first;
        Tile second;

        public List<List<Tile>> TilesAsListOfLines
        {
            get
            {
                var result = new List<List<Tile>>();
                for (int i = 0; i < Tiles.GetLength(0); i++)
                {
                    var newList = new List<Tile>();
                    for (int k = 0; k < Tiles.GetLength(1); k++)
                        newList.Add(Tiles[i, k]);
                    result.Add(newList);
                }
                return result;
            }
        }

        #region Временный костыль ввиду отсутсвия UserInput
        public void OnTileClick(Tile tile)
        {
            OnNewTileSelect(tile);
        }

        void OnNewTileSelect(Tile tile)
        {
            if (first == tile || second == tile)
                ResetAllSelectedTiles();
            else if (first == null)
                first = SetSelectionTile(tile);
            else if (second == null)
                second = SetSelectionTile(tile);
            else
                ResetAllSelectedTiles();
            if (first != null && second != null)
            {
                onSelectedTwoChips?.Invoke(first, second);
                ResetAllSelectedTiles();
                return;
            }
            onUserSelectChip?.Invoke();
        }
        #endregion

        public void ResetAllSelectedTiles()
        {
            first = ResetSelectedTile(first);
            second = ResetSelectedTile(second);
        }

        Tile SetSelectionTile(Tile tile)
        {
            tile.Select();
            return tile;
        }

        Tile ResetSelectedTile(Tile tile)
        {
            if (tile != null)
                tile.Deselect();
            return null;
        }

        public void CreateBoard()
        {
            if (Generator == null)
                Generator = GetComponent<TileGenerator>();

            CheckBoardRoot();

            if (Tiles.IsNullOrDefault() || Tiles.GetLength(0) != boardSize.x || Tiles.GetLength(1) != boardSize.y)
                Tiles = new Tile[boardSize.x, boardSize.y];

            TileOffset = Generator.GetTileBoundsSize;

            for (int x = 0; x < boardSize.x; x++)
                for (int y = 0; y < boardSize.y; y++)
                    Tiles[x, y] = InitTile(Tiles, x, y);
        }

        Tile InitTile(Tile[,] tiles, int x, int y)
        {
            var tile = tiles[x, y];

            if (tile.IsNullOrDefault())
                return InitNewTile(position: new Vector2Int(x, y), generator: Generator);

            var nearestTypes = GetNearestTileTypes(new Vector2Int(x, y));
            var newData = Generator.GenerateTileData(nearestTypes);

            tile.ApplyNewData(newData);
            tile.transform.localPosition = new Vector3(TileOffset.x * x, TileOffset.y * boardSize.y, 0);

            return tile;
        }

        Tile InitNewTile(Vector2Int position, TileGenerator generator)
        {
            var tile = generator.GenerateNewTile(GetNearestTileTypes(position));
            tile.gameObject.name = $"Tile [{position.x}/{position.y}]";
            tile.transform.SetParent(boardRoot);
            tile.position = position;
            tile.transform.localPosition = new Vector3(TileOffset.x * position.x, TileOffset.y * position.y, 0);
            return tile;
        }

        void CheckBoardRoot()
        {
            if (!boardRoot.IsNullOrDefault())
                return;
            boardRoot = new GameObject("TileRoot").transform;
            boardRoot.SetParent(transform);
            boardRoot.localPosition = Vector3.zero;
        }

        public TileType[] GetNearestTileTypes(Vector2Int position)
        {
            // Костыль вырезаем дебаговые цвета
            // TODO: Механизм регулировки сложности
            var result = new List<TileType>() {
                TileType.NONE,
                //TileType.white,
                //TileType.black,

                //TileType.pink,
                //TileType.orange,
                //TileType.purple,
                //TileType.yellow,

                //TileType.blue,
                //TileType.grey,
            };

            for (int i = 0; i < BaseValues.directions.Length; i++)
            {
                var tile = GetTileByPosition(position + BaseValues.directions[i]);
                if (!tile.IsNullOrDefault() && !tile.tileData.IsNullOrDefault())
                    if (!result.Contains(tile.GetTileTypeSafe()))
                        result.Add(tile.GetTileTypeSafe());
            }
            return result.Count > 0 ? result.ToArray() : null;
        }

        Tile GetTileByPosition(Vector2Int position)
        {
            if (!BaseExtensions.IsPointBelongsSite(position, new Rect(Vector2Int.zero, boardSize)))
                return null;

            return Tiles[position.x, position.y];
        }
    }
}
