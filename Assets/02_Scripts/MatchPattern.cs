using System.Collections.Generic;
using UnityEngine;

namespace M3T
{
    /// <summary>
    /// Содержит набор паттернов для матчинга фишек
    /// </summary>
    public class MatchPattern : MonoBehaviour
    {
        public List<List<IMatchingPattern>> patterns = new List<List<IMatchingPattern>>();

        private void Awake()
        {
            patterns.Add(bonusMatchingPatterns);
            patterns.Add(baseMatchingPatterns);
            patterns.Add(basePredictedPatterns);
        }

        public List<IMatchingPattern> bonusMatchingPatterns = new List<IMatchingPattern>()
        {
             /// <summary>
            /// 000X000
            /// 000X000
            /// 0XXXXX0
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),
                    new Vector2Int(3,0),
                    new Vector2Int(4,0),
                    new Vector2Int(2,1),
                    new Vector2Int(2,2),
                },
                weight: 14),
            /// <summary>
            /// 0XXXXX0
            /// 000X000
            /// 000X000
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),
                    new Vector2Int(3,0),
                    new Vector2Int(4,0),
                    new Vector2Int(2,-1),
                    new Vector2Int(2,-2),
                },
                weight: 14),
            /// <summary>
            /// 0X000
            /// 0X000
            /// 0XXX0
            /// 0X000
            /// 0X000
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                    new Vector2Int(0,3),
                    new Vector2Int(0,4),
                    new Vector2Int(1,2),
                    new Vector2Int(2,2),
                },
                weight: 14),
            /// <summary>
            /// 000X0
            /// 000X0
            /// 0XXX0
            /// 000X0
            /// 000X0
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                    new Vector2Int(0,3),
                    new Vector2Int(0,4),
                    new Vector2Int(-1,2),
                    new Vector2Int(-2,2),
                },
                weight: 13),
            /// <summary>
            /// 000X0
            /// 000X0
            /// 0XXX0
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),
                    new Vector2Int(2,1),
                    new Vector2Int(2,2),
                },
                weight: 12),
            /// <summary>
            /// 0XXX0
            /// 0X000
            /// 0X000
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                    new Vector2Int(1,2),
                    new Vector2Int(2,2),
                },
                weight: 12),
            /// <summary>
            /// X00
            /// X00
            /// X00
            /// X00
            /// X00
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                    new Vector2Int(0,3),
                    new Vector2Int(0,4),
                },
                weight: 11),
            /// <summary>
            /// X00
            /// X00
            /// X00
            /// X00
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                    new Vector2Int(0,3),
                },
                weight: 10),
             /// <summary>
            /// XXXXX
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),
                    new Vector2Int(3,0),
                    new Vector2Int(4,0),
                },
                weight: 11),
            /// <summary>
            /// XXXX
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),
                    new Vector2Int(3,0),
                },
                weight: 10),
            /// <summary>
            /// 0XX0
            /// 0XX0
            /// </summary>
            new BonusMatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(1,1),
                    new Vector2Int(0,1),
                },
                weight: 10),
        };

        public List<IMatchingPattern> baseMatchingPatterns = new List<IMatchingPattern>()
        {
            
            /// <summary>
            /// X00
            /// X00
            /// X00
            /// </summary>
            new MatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                }),
            /// <summary>
            /// XXX
            /// </summary>
            new MatchingPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),
                }),
        };

        public List<IMatchingPattern> basePredictedPatterns = new List<IMatchingPattern>()
        {
#region Verticals
            /// <summary>
            /// X00
            /// 000
            /// X00
            /// X00
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,3),
                },
                movedTile: new PredictionPatternData(new Vector2Int(0,3), Vector2Int.down)),
            /// <summary>
            /// X00
            /// X00
            /// 000
            /// X00
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,2),
                    new Vector2Int(0,3),
                },
                movedTile:new PredictionPatternData(new Vector2Int(0,0), Vector2Int.up)),
            /// <summary>
            /// 0X0
            /// X00
            /// X00
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(1,2),
                },
                movedTile: new PredictionPatternData(new Vector2Int(1,2), Vector2Int.left)),
            /// <summary>
            /// X00
            /// 0X0
            /// 0X0
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(-1,2),
                },
                movedTile: new PredictionPatternData(new Vector2Int(-1,2), Vector2Int.right)),
            /// <summary>
            /// X00
            /// 0X0
            /// X00
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,1),
                    new Vector2Int(0,2),
                },
                movedTile:  new PredictionPatternData(new Vector2Int(1,1), Vector2Int.left)),
            /// <summary>
            /// 0X0
            /// X00
            /// 0X0
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(-1,1),
                    new Vector2Int(0,2),
                },
                movedTile: new PredictionPatternData(new Vector2Int(-1,1), Vector2Int.right)),
            /// <summary>
            /// 0X0
            /// 0X0
            /// X00
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,1),
                    new Vector2Int(1,2),
                },
                movedTile: new PredictionPatternData(new Vector2Int(0,0), Vector2Int.right)),
            /// <summary>
            /// X00
            /// X00
            /// 0X0
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(-1,1),
                    new Vector2Int(-1,2),
                },
                movedTile: new PredictionPatternData(new Vector2Int(0,0), Vector2Int.left)),
#endregion
#region Horizontals
            /// <summary>
            /// XX0X
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(3,0),
                },
                movedTile: new PredictionPatternData(new Vector2Int(3,0), Vector2Int.left)),
            /// <summary>
            /// X0XX
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(2,0),
                    new Vector2Int(3,0),
                },
                movedTile: new PredictionPatternData(new Vector2Int(0,0), Vector2Int.right)),
            /// <summary>
            /// 0X0
            /// X0X
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,1),
                    new Vector2Int(2,0),
                },
                movedTile: new PredictionPatternData(new Vector2Int(1,1), Vector2Int.down)),
            /// <summary>
            /// X0X
            /// 0X0
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,-1),
                    new Vector2Int(2,0),
                },
                movedTile: new PredictionPatternData(new Vector2Int(1,-1), Vector2Int.up)),
            /// <summary>
            /// X00
            /// 0XX
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,-1),
                    new Vector2Int(2,-1),
                },
                movedTile: new PredictionPatternData(new Vector2Int(0,0), Vector2Int.down)),
            /// <summary>
            /// 0XX
            /// X00
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,1),
                    new Vector2Int(2,1),
                },
                movedTile: new PredictionPatternData(new Vector2Int(0,0), Vector2Int.up)),
            /// <summary>
            /// 00X
            /// XX0
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,1),
                },
                movedTile: new PredictionPatternData(new Vector2Int(2,1), Vector2Int.down)),
            /// <summary>
            /// XX0
            /// 00X
            /// </summary>
            new PredictionPattern(vectors:
                new Vector2Int[]
                {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,-1),
                },
                movedTile: new PredictionPatternData(new Vector2Int(2,-1), Vector2Int.up)),
#endregion
        };
    }
}
