using System;
using System.Collections.Generic;
using M3T;
using M3T.Animations;

namespace FSM
{
    public class CheckMatchesState : AbsStateFSM, IStateFSM<CheckMatchesState>
    {
        public StatesEnum State => StatesEnum.FindMatches;
        public string StateName => State.ToString();
        public override string ComponentName => typeof(CheckMatchesState).FullName;

        List<List<Tile>> matches = null;
        List<PredictionData> predictions = null;

        public override void Enter()
        {
            base.Enter();
            CheckMatches();
            if (matches != null)
            {
                CreateExplodeAnimation(ExplodeAnimationEnd);
                CreateFallingAnimation(FallingAnimationEnd);
                return;
            }

            CheckPredictions();
            if (predictions != null)
                gameController.ControllerFSM.SetState(StatesEnum.WaitUserInput);
            else
                gameController.ControllerFSM.SetState(StatesEnum.HaveNoOneTurn);
        }

        public override void Exit()
        {
            matches = null;
            predictions = null;
        }

        void CheckMatches()
        {
            matches = gameController.Matcher.FoundMatches(gameController.Board.Tiles);
            matches = matches.Count > 0 ? matches : null;
        }

        void CheckPredictions()
        {
            predictions = gameController.Matcher.PredictMatches(gameController.Board.Tiles);
            predictions = predictions.Count > 0 ? predictions : null;
        }

        public void ExplodeAnimationEnd()
        {
            this.Log($"End anim explode");
            gameController.Matcher.RemoveExplodedChips(matches, gameController.Board);
        }

        public void FallingAnimationEnd()
        {
            this.Log($"End anim falling");
            gameController.ControllerFSM.SetState(StatesEnum.FindMatches);
        }

        public void CreateExplodeAnimation(Action callback)
        {
            gameController.Animator.AddAnimation(new ExplodeChipAnimation(matches)
            {
                duration = gameController.AnimationsTimeSettings.ExplodingDuration,
                Start = () => { this.Log($"Start anim explode match"); },
                End = callback,
            });
        }

        public void CreateFallingAnimation(System.Action callback)
        {
            var fallingSpeed = gameController.AnimationsTimeSettings.FallingSpeed;
            gameController.Animator.AddAnimation(new AnimationFallingFixSpeed(matches, fallingSpeed: fallingSpeed)
            {
                duration = gameController.AnimationsTimeSettings.FallingDelayBetweenLines,
                Start = () => this.Log($"[CheckMatchesState] Start anim falling"),
                End = callback,
            });
        }
    }
}