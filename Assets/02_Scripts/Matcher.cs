using System.Collections.Generic;
using UnityEngine;

namespace M3T
{
    /// <summary>
    /// // Основные правила игры.
    /// Отвечает за:
    ///      1. Успешность/неуспешность передвижения фишек
    ///      2. Поиск матчей среди всей доски
    /// </summary>
    public class Matcher : MonoBehaviour
    {
        MatchPattern matchPattern;

        private void Start()
        {
            matchPattern = GetComponent<MatchPattern>();
        }

        bool CheckChangePossibility(Vector2Int first, Vector2Int second)
        {
            return (second - first).magnitude == 1;
        }

        protected virtual List<List<Tile>> FoundMatchesAfterSwipe(Tile[,] tiles, Tile tile1, Tile tile2)
        {
            var result = new List<List<Tile>>();
            var rect = new Rect(Vector2Int.zero, new Vector2Int(tiles.GetLength(0), tiles.GetLength(1)));

            //Временное решение!!

            { // Check bonus patterns with chip1
                var matchesTile = CheckPatternType(matchPattern.bonusMatchingPatterns, tiles, tile1, rect);
                if (matchesTile != null && matchesTile.Count > 0)
                {
                    var match = new List<Tile>();
                    match.AddRange(matchesTile.ToArray());
                    result.Add(match);
                }
                if (result.Count > 0)
                    return result;
            }

            {   // Check bonus patterns with chip2
                var matchesTile = CheckPatternType(matchPattern.bonusMatchingPatterns, tiles, tile2, rect);
                if (matchesTile != null && matchesTile.Count > 0)
                {
                    var match = new List<Tile>();
                    match.AddRange(matchesTile.ToArray());
                    result.Add(match);
                }
                if (result.Count > 0)
                    return result;
            }

            { // Check bonus patterns with all chips
                var matchesTile = CheckPatternType(matchPattern.bonusMatchingPatterns, tiles, rect);
                if (matchesTile != null && matchesTile.Count > 0)
                {
                    var matches = matchesTile.ToArray();
                    for (int i = 0; i < matches.Length; i++)
                    {
                        var match = new List<Tile>();
                        match.AddRange(matches[i]);
                        result.Add(match);
                    }
                }
                if (result.Count > 0)
                    return result;
            }

            {   // Check standart patterns with chip1
                var matchesTile = CheckPatternType(matchPattern.baseMatchingPatterns, tiles, tile1, rect);
                if (matchesTile != null && matchesTile.Count > 0)
                {
                    var match = new List<Tile>();
                    match.AddRange(matchesTile.ToArray());
                    result.Add(match);
                }
                if (result.Count > 0)
                    return result;
            }

            {   // Check standart patterns with chip2
                var matchesTile = CheckPatternType(matchPattern.baseMatchingPatterns, tiles, tile2, rect);
                if (matchesTile != null && matchesTile.Count > 0)
                {
                    var match = new List<Tile>();
                    match.AddRange(matchesTile.ToArray());
                    result.Add(match);
                }
                if (result.Count > 0)
                    return result;
            }

            {   // Check standart patterns with all chips
                var matchesTile = CheckPatternType(matchPattern.baseMatchingPatterns, tiles, rect);
                if (matchesTile != null && matchesTile.Count > 0)
                {
                    var matches = matchesTile.ToArray();
                    for (int i = 0; i < matches.Length; i++)
                    {
                        var match = new List<Tile>();
                        match.AddRange(matches[i]);
                        result.Add(match);
                    }
                }
                if (result.Count > 0)
                    return result;
            }

            return result;
        }

        public virtual List<List<Tile>> FoundMatches(Tile[,] tiles)
        {
            var result = new List<List<Tile>>();
            var rect = new Rect(Vector2Int.zero, new Vector2Int(tiles.GetLength(0), tiles.GetLength(1)));

            //Временное решение!!

            {   // Check bonus patterns with all chips
                var matchesTile = CheckPatternType(matchPattern.bonusMatchingPatterns, tiles, rect);
                if (matchesTile != null && matchesTile.Count > 0)
                {
                    var matches = matchesTile.ToArray();
                    for (int i = 0; i < matches.Length; i++)
                    {
                        var match = new List<Tile>();
                        match.AddRange(matches[i]);
                        result.Add(match);
                    }
                }
                if (result.Count > 0)
                    return result;
            }

            {   // Check standart patterns with all chips
                var matchesTile = CheckPatternType(matchPattern.baseMatchingPatterns, tiles, rect);
                if (matchesTile != null && matchesTile.Count > 0)
                {
                    var matches = matchesTile.ToArray();
                    for (int i = 0; i < matches.Length; i++)
                    {
                        var match = new List<Tile>();
                        match.AddRange(matches[i]);
                        result.Add(match);
                    }
                }
                if (result.Count > 0)
                    return result;
            }

            return result;
        }

        public void SwipeChipsSilent(Tile tile1, Tile tile2)
        {
            var cache = tile1.tileData;
            tile1.ApplyNewData(tile2.tileData);
            tile2.ApplyNewData(cache);
        }

        // БЕЗ АНИМАЦИЙ!
        // Проверяет возможен ли свайп фишек и возвращает список матчей, если свайп успешен.
        public List<List<Tile>> FindMatchesIfSwipeSuccess(Tile[,] tiles, Tile tile1, Tile tile2)
        {
            // Проверяем, что фишки соседствующие
            if (!CheckChangePossibility(tile1.position, tile2.position))
                return null;
            // Совершаем обмен
            SwipeChipsSilent(tile1, tile2);
            // Проверяем, что обмен привел к матчу
            // Сохраняем найденные матчи
            var matches = FoundMatchesAfterSwipe(tiles, tile1, tile2);
            // Совершаем обмен обратно
            SwipeChipsSilent(tile1, tile2);
            return matches;
        }

        // Выглядит костыльно, !!
        // НЕ ПОДДЕРЖИВАЕТ ПРОИЗВОЛЬНОЕ НАПРАВЛЕНИЕ ПАДЕНИЯ 
        public List<List<Tile>> RemoveExplodedChips(List<List<Tile>> matches, IBoard board)
        {
            var m = new List<Tile>();
            for (int i = 0; i < matches.Count; i++)
                for (int j = 0; j < matches[i].Count; j++)
                {
                    var tile = matches[i][j];
                    tile.tileData = null;
                    m.Add(tile);
                }

            matches.Clear();
            for (int i = 0; i < board.Tiles.GetLength(1); i++)
                matches.Add(new List<Tile>());

            m.Sort((x1, x2) => x1.position.y < x2.position.y ? -1 : x1.position.y == x2.position.y ? 0 : 1);
            while (m.Count > 0)
            {
                var tile = m[0];
                m.Remove(tile);
                matches[tile.position.y].Add(tile);
                var next = FindNext(tile, board.Tiles);
                /*
                 * Мы получили следующую НОРМАЛЬНУЮ сверху фишку,
                 * Нужно текущей отдать дату следующей и присвоить ее позицию,
                 * После отдать аниматору этот же список матчей,
                 * Чтобы аниматор вернул фишки на их места.
                 */
                if (next == null)
                {
                    GenerateNewData(tile, board);
                }
                else
                {
                    ApplyTileDataByNext(tile, next);
                    m.Add(next);
                    m.Sort((x1, x2) => x1.position.y < x2.position.y ? -1 : x1.position.y == x2.position.y ? 0 : 1);
                }
            }
            return matches;
        }

        void ApplyTileDataByNext(Tile tile, Tile next)
        {
            tile.ApplyNewData(next.tileData);
            // Нужно вгенерировать новую дату для Следующего тайла
            // Т.к. его даты мы отоблрали
            next.ApplyNewData(null);
            tile.transform.localPosition = next.transform.localPosition;
        }

        void GenerateNewData(Tile tile, IBoard board)
        {
            var exceptedTileTypes = board.GetNearestTileTypes(tile.position);
            var newData = board.Generator.GenerateTileData(exceptedTileTypes);
            tile.ApplyNewData(newData);
            tile.transform.localPosition = board.Tiles[tile.position.x, board.Tiles.GetLength(1) - 1].transform.localPosition + board.TileOffset.y * Vector3.up;
        }

        Tile FindNext(Tile tile, Tile[,] tiles)
        {
            if (tile.IsNullOrDefault())
                return null;
            var searchDirection = Vector2Int.up;
            var targetPosition = tile.position + searchDirection;
            if (!BaseExtensions.IsPointBelongsSite(targetPosition, new Rect(Vector2Int.zero, new Vector2Int(tiles.GetLength(0), tiles.GetLength(1)))))
                return null;
            var targetTile = tiles[targetPosition.x, targetPosition.y];
            if (targetTile.tileData.IsNullOrDefault() || targetTile.GetTileTypeSafe() == TileType.NONE)
                targetTile = FindNext(targetTile, tiles);
            return targetTile;
        }

        /// <summary>
        /// Возвращает список фишек, которые нужно подсветить
        /// Фишку, которую нужно анимировать движением и вектор, в какую сторону
        /// </summary>
        /// <param name="tiles"></param>
        /// <returns></returns>
        public List<PredictionData> PredictMatches(Tile[,] tiles)
        {
            var rect = new Rect(Vector2Int.zero, new Vector2Int(tiles.GetLength(0), tiles.GetLength(1)));

            #region Дебажный костыль !! Подсвечивает ВСЕ предикшены
            for (int i = 0; i < rect.xMax; i++)
                for (int k = 0; k < rect.yMax; k++)
                    tiles[i, k].SetPredictionSelection(false);
            #endregion

            return CheckPredictionPatterns(matchPattern.basePredictedPatterns, tiles, rect, "Prediction");
        }

        Tile GetTile(Tile[,] tiles, Rect tilesArraySize, Vector2Int targetPosition)
        {
            if (BaseExtensions.IsPointBelongsSite(targetPosition, tilesArraySize))
                return tiles[targetPosition.x, targetPosition.y];
            return null;
        }

        Tile GetNextTile(Tile[,] tiles, Rect tilesArraySize, Vector2Int currentPostion)
        {
            if (currentPostion.x < tilesArraySize.xMax)
                if (currentPostion.y + 1 < tilesArraySize.yMax)
                    return GetTile(tiles, tilesArraySize, new Vector2Int(currentPostion.x, currentPostion.y + 1));
                else
                    return GetNextTile(tiles, tilesArraySize, new Vector2Int(currentPostion.x + 1, -1));
            return null;
        }

        HashSet<HashSet<Tile>> CheckPatternType(List<IMatchingPattern> patterns, Tile[,] tiles, Rect tilesArraySize)
        {
            var matches = new List<(int weight, HashSet<Tile> matches)>();
            Tile currentTile;
            Vector2Int currentPosition = Vector2Int.down;
            while ((currentTile = GetNextTile(tiles, tilesArraySize, currentPosition)) != null)
            {
                currentPosition = currentTile.position;
                for (int i = 0; i < patterns.Count; i++)
                {
                    var pattern = patterns[i];
                    var foundedMatches = CheckPattern(pattern.vectors, tiles, currentTile, tilesArraySize);
                    if (foundedMatches == null || foundedMatches.Count == 0)
                        continue;
                    if (foundedMatches.Count >= pattern.vectors.Length)
                        matches.Add((weight: pattern.weight, matches: foundedMatches));
                }
            }
            if (matches.Count == 0)
                return null;
            var alreadyMatched = new HashSet<Tile>();
            matches.Sort((x1, x2) => x1.weight > x2.weight ? -1 : x1.weight == x2.weight ? 0 : 1);
            var result = new HashSet<HashSet<Tile>>();
            for (int i = 0; i < matches.Count; i++)
            {
                var currentMatch = matches[i].matches;
                var prevCount = currentMatch.Count;
                currentMatch.ExceptWith(alreadyMatched);
                if (currentMatch.Count != prevCount)
                    continue;
                alreadyMatched.UnionWith(currentMatch);
                result.Add(currentMatch);
            }

            return result;
        }

        HashSet<Tile> CheckPatternType(List<IMatchingPattern> patterns, Tile[,] tiles, Tile currentTile, 
            Rect tilesArraySize, string patternName = null)
        {
            var matches = new HashSet<Tile>();
            for (int i = 0; i < patterns.Count; i++)
            {
                var pattern = patterns[i];
                var foundedMatches = CheckPattern(pattern.vectors, tiles, currentTile, tilesArraySize);
                if (foundedMatches != null && foundedMatches.Count >= pattern.vectors.Length)
                    matches.UnionWith(foundedMatches);
            }
            return matches;
        }

        List<PredictionData> CheckPredictionPatterns(List<IMatchingPattern> patterns, Tile[,] tiles, 
            Rect tilesArraySize, string patternName = null)
        {
            var matches = new HashSet<HashSet<Tile>>();
            var result = new List<PredictionData>();
            Tile currentTile = null;
            var currentPosition = Vector2Int.down;
            while ((currentTile = GetNextTile(tiles, tilesArraySize, currentPosition)) != null)
            {
                currentPosition = currentTile.position;
                for (int i = 0; i < patterns.Count; i++)
                {
                    if (!(patterns[i] is PredictionPattern))
                        continue;

                    var pattern = (PredictionPattern)patterns[i];

                    var foundedMatches = CheckPattern(pattern.vectors, tiles, currentTile, tilesArraySize);
                    if (!foundedMatches.IsNullOrDefault() && foundedMatches.Count >= pattern.vectors.Length)
                    {
                        if (matches.Add(foundedMatches))
                            result.Add( new PredictionData()
                            {
                                tiles = foundedMatches.ToArray(),
                                target = GetTile(tiles, tilesArraySize, currentPosition + pattern.movedTile.movedPosition),
                                direction = pattern.movedTile.movedDirection
                            });
                    }
                }
            }
            matches.Clear();
            matches = null;
            return result;
        }

        HashSet<Tile> CheckPattern(Vector2Int[] vectors, Tile[,] tiles, Tile current, Rect tilesArraySize)
        {
            var result = new HashSet<Tile>() { current };
            // костыль! возможно нужно убрать (0,0) из паттернов
            // Первый элемент в массивах векторов в паттернах - всегда текущая позиция.
            for (int y = 1; y < vectors.Length; y++)
            {
                var targetTile = GetTile(tiles, tilesArraySize, current.position + vectors[y]);
                if (BaseValues.IsCompareTileTypes(current.GetTileTypeSafe(), targetTile.GetTileTypeSafe()))
                    result.Add(targetTile);
                else
                    break;
            }
            if (result.Count >= vectors.Length)
                return result;
            else return null;
        }
    }
}