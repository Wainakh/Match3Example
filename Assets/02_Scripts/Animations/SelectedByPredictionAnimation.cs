using System.Collections;
using UnityEngine;
using FSM;
using System;

namespace M3T.Animations
{
    public class SelectedByPredictionAnimation : Animation, IParallelAnimation, IInfinityAnimation, 
        IStateFSM<SelectedByPredictionAnimation>
    {
        public StatesEnum State => StatesEnum.WaitUserInput;
        public string StateName => State.ToString() + "_SelectPrediction";

        public Action Subscribe { get; set; }
        public Action Unsubscribe { get; set; }

        public event Action isInterrupted;

        Transform tile;
        Vector3 direction;
        Vector3 defaultPosition;
        float delayTime;

        public void Enter() { }
        public void Exit() { }

        public SelectedByPredictionAnimation()
        {
            duration = GameController.Instance.AnimationsTimeSettings.PredictionMove;
            delayTime = GameController.Instance.AnimationsTimeSettings.PredictionDelay;
        }

        public SelectedByPredictionAnimation(Transform tile, Vector2 direction) : this()
        {
            this.tile = tile;
            defaultPosition = this.tile.localPosition;
            this.direction = new Vector3(direction.x, direction.y, 0);
        }

        public override IEnumerator Work()
        {
            yield return new WaitForSeconds(delayTime);
            Subscribe?.Invoke();
            yield return Working();
        }

        public IEnumerator Working()
        {
            var waitForSeconds = new WaitForSeconds(duration - (duration * .4f));
            while (true)
            {
                yield return (MoveTransformToDirectionWithFixedDurationAnimation(tile, direction, duration * .1f));
                yield return (MoveTransformToDirectionWithFixedDurationAnimation(tile, -direction, duration * .1f));

                yield return (MoveTransformToDirectionWithFixedDurationAnimation(tile, direction, duration * .1f));
                yield return (MoveTransformToDirectionWithFixedDurationAnimation(tile, -direction, duration * .1f));

                yield return waitForSeconds;
            }
        }

        public void Interrupt()
        {
            tile.localPosition = defaultPosition;
            isInterrupted?.Invoke();
        }
    }
}
