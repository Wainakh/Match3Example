using System.Collections.Generic;
using UnityEngine;
using M3T;
using M3T.Animations;
using System;

namespace FSM
{
    public class WaitUserInputState : AbsStateFSM, IStateFSM<WaitUserInputState>
    {
        public StatesEnum State => StatesEnum.WaitUserInput;
        public string StateName => State.ToString();
        public override string ComponentName => typeof(WaitUserInputState).FullName;

        List<Action> interruptHandlers = new List<Action>();
        List<Tile> predictedTiles = new List<Tile>();

        public override void Enter()
        {
            base.Enter();
            var predictions = gameController.Matcher.PredictMatches(gameController.Board.Tiles);
            SelectPredictions(predictions, gameController.Board);
        }

        public override void Exit()
        {
            for (int i = 0; i < predictedTiles.Count; i++)
                predictedTiles[i].SetPredictionSelection(false);
            predictedTiles.Clear();

            for (int i = 0; i < interruptHandlers.Count; i++)
                interruptHandlers[i]?.Invoke();
            interruptHandlers.Clear();
        }

        public void SelectPredictions(List<PredictionData> predictions, IBoard board)
        {
            var alreadyAnimated = new HashSet<Tile>();
            for (int i = 0; i < predictions.Count; i++)
            {
                var prediction = predictions[i];
                if (alreadyAnimated.Add(prediction.target))
                    AnimatePredictedTile(board, predictions[i].target, prediction.direction, startAction: () =>
                    {
                        for (int k = 0; k < prediction.tiles.Length; k++)
                        {
                            predictedTiles.Add(prediction.tiles[k]);
                            prediction.tiles[k].SetPredictionSelection(true);
                        }
                    });
            }
            alreadyAnimated = null;
            gameController.Animator.Run();
        }

        void AnimatePredictedTile(IBoard board, Tile target, Vector2 direction, Action startAction)
        {
            var predictedTileMoveKoeff = .2f;
            direction *= board.Generator.GetTileBoundsSize * predictedTileMoveKoeff;
            var anim = new SelectedByPredictionAnimation(target.transform, direction);
            anim.Subscribe = startAction;
            Action handler = () =>
            {
                anim.Interrupt();
                anim = null;
            };
            interruptHandlers.Add(handler);
            gameController.Animator.AddAnimation(anim, playAwake: false);
        }
    }
}