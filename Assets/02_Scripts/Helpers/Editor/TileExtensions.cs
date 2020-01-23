using M3T;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TileExtensions
{
    [MenuItem("CONTEXT/Tile/Apply new Data SILENT")]
    static void ApplyNewDataSilent(MenuCommand command)
    {
        Tile tile = (Tile)command.context;
        if (tile != null)
            tile.ApplyNewData(new TileData()
            {
                tileType = tile.GetTileTypeSafe(),
                color = BaseValues.ColorsByType[tile.GetTileTypeSafe()],
                sprite = null,
            });
    }
}