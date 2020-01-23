using System.Collections.Generic;
using UnityEngine;

namespace M3T
{
    /// <summary>
    /// Отвечает за генерацию новых фишек
    /// </summary>
    public class TileGenerator : MonoBehaviour
    {
        public Vector2 GetTileBoundsSize => TilePrefab.GetComponent<Tile>().GetSize();

        [SerializeField] GameObject tilePrefab;
        GameObject TilePrefab => !tilePrefab.IsNullOrDefault() ? tilePrefab :
            tilePrefab = Resources.Load<GameObject>("TilePrefab");

        public Tile GenerateNewTile(TileType[] nearest)
        {
            var tile = Instantiate(TilePrefab).GetComponent<Tile>(); //TODO: Replace to AddOnce
            var curTileData = GenerateTileData(nearest);
            tile.ApplyNewData(curTileData);
            return tile;
        }

        public TileData GenerateTileData(TileType[] nearest)
        {
            var tileData = new TileData();
            var enums = new List<TileType>(System.Enum.GetValues(typeof(TileType)) as TileType[]);
            if (!nearest.IsNullOrDefault())
                for (int i = 0; i < nearest.Length; i++)
                    if (enums.Count > 1)
                        enums.Remove(nearest[i]);
            tileData.tileType = enums[Random.Range(0, enums.Count)];
            tileData.color = BaseValues.ColorsByType[tileData.tileType];
            tileData.sprite = null; 

            return tileData;
        }
    }
}
