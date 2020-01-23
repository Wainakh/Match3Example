using M3T;
using M3T.Animations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class HaveNoOneTurnState : GenerationState, IStateFSM<HaveNoOneTurnState>
    {
        int x1, y1, x2, y2;
        Tile tile1 = null;
        Tile tile2 = null;
        HashSet<(Tile, Tile)> tryed;

        public StatesEnum State => StatesEnum.HaveNoOneTurn;
        public string StateName => State.ToString();
        public override string ComponentName => typeof(HaveNoOneTurnState).FullName;

        public override void Enter()
        {
            base.Enter();
            var result = ChangeChips();
            this.Log($"[{Time.realtimeSinceStartup}] [ChangeChips] finish. Tryed is [{tryCount}]");

            if (!result.IsNullOrDefault() 
                && !result.tile1.IsNullOrDefault() && !result.tile2.IsNullOrDefault())
                CreateAnimation(result.tile1, result.tile2);
            else
            {
                CreateExplodeAnimation(ExplodeAnimationEnd);
                CreateFallingAnimation(FallingAnimationEnd);
            }
        }

        public override void Exit() { }

        UserSelectionData ChangeChips()
        {
            var size = gameController.Board.BoardSize;
            tryCount++;

            if (tryed == null)
                tryed = new HashSet<(Tile, Tile)>();

            while (tile1.IsNullOrDefault() || tile2.IsNullOrDefault() 
                || tile1 == tile2 || tryed.Contains((tile1, tile2)))
            {
                tile1 = gameController.Board.Tiles[x1, y1];
                tile2 = gameController.Board.Tiles[x2, y2];
                if (!Increase())
                    return default;
            }

            gameController.Matcher.SwipeChipsSilent(tile1, tile2);

            var matches = gameController.Matcher.FoundMatches(gameController.Board.Tiles);
            if (matches.Count > 0)
            {
                AddSequence(tryed, tile1, tile2);
                return ChangeChips();
            }

            var predict = gameController.Matcher.PredictMatches(gameController.Board.Tiles);
            if (predict.Count == 0)
            {
                AddSequence(tryed, tile1, tile2);
                return ChangeChips();
            }

            gameController.Matcher.SwipeChipsSilent(tile1, tile2);
            tryed = null;
            return new UserSelectionData(tile1, tile2);
        }

        void AddSequence(HashSet<(Tile, Tile)> tryed, Tile tile1, Tile tile2)
        {
            tryed.Add((tile2, tile1));
            tryed.Add((tile1, tile2));
            gameController.Matcher.SwipeChipsSilent(tile1, tile2);
        }

        bool Increase()
        {
            var cont = GameController.Instance;
            var size = cont.Board.BoardSize;
            y2++;
            if (y2 >= size.y)
            {
                x2++;
                y2 = 0;
            }
            if (x2 >= size.x)
            {
                y1++;
                x2 = y2 = 0;
            }
            if (y1 >= size.y)
            {
                x1++;
                y1 = 0;
            }
            if (x1 >= size.x)
            {
                x1 = y1 = x2 = y2 = 0;
                return false;
            }
            return true;
        }

        void CreateAnimation(Tile tile1, Tile tile2)
        {
            gameController.Animator.AddAnimation(new SwipeTwoChipsSuccessAnimation(tile1.transform, tile2.transform,
                CheckPoint: () => gameController.Matcher.SwipeChipsSilent(tile1, tile2))
            {
                duration = gameController.AnimationsTimeSettings.SwipeMoveDurationHaveNoTurn,
                Start = () => this.Log($"Start swipe animation. t1[{tile1.name}] => t2[{tile2.name}]"),
                End = () =>
                {
                   this.Log($"End swipe animation");
                   gameController.ControllerFSM.SetState(StatesEnum.WaitUserInput);
                }
            });
        }

        public void ExplodeAnimationEnd()
        {
            this.Log("End anim explode");
            ReGenerateBoard();
        }

        public void FallingAnimationEnd()
        {
            this.Log("End anim falling");
            gameController.ControllerFSM.SetState(StatesEnum.FindMatches);
        }

        public void CreateExplodeAnimation(Action callback)
        {
            gameController.Animator.AddAnimation(new ExplodeChipAnimation(gameController.Board.TilesAsListOfLines)
            {
                duration = gameController.AnimationsTimeSettings.ExplodingDuration,
                Start = () => this.Log("Start anim explode match"),
                End = callback,
            });
        }

        public void CreateFallingAnimation(Action callback)
        {
            gameController.Animator.AddAnimation(new AnimationFallingFixSpeed(gameController.Board.TilesAsListOfLines, 
                fallingSpeed: gameController.AnimationsTimeSettings.FallingSpeed)
            {
                duration = gameController.AnimationsTimeSettings.FallingDelayBetweenLines,
                Start = () => this.Log("[HaveNoOneTurnState]Start anim falling"),
                End = callback,
            });
        }
    }
}