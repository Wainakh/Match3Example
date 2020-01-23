using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M3T.Animations
{
    public class TileAnimator : MonoBehaviour
    {
        public event Action allAnimationsEnds;

        Queue<Animation> animationsQueue = new Queue<Animation>();
        bool isPlaying = false;
        Coroutine working;

        public void Run()
        {
            Stop();
            isPlaying = true;
            working= StartCoroutine(Work());
        }

        public void Pause() { }

        public void Stop()
        {
            var enumerator = animationsQueue.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var animation = enumerator.Current as IInfinityAnimation;
                if (animation.IsNullOrDefault())
                    continue;
                animation.Interrupt();
            }

            if (!working.IsNullOrDefault())
                StopCoroutine(working);
        }

        public void AddAnimation(Animation animation, bool playAwake = true)
        {
            animationsQueue.Enqueue(animation);
            animation.coroutineRoot = this;
            if (!isPlaying && playAwake)
                Run();
        }

        IEnumerator Work()
        {
            this.Log("Animator Start working");

            var standartDelay = GameController.Instance.AnimationsTimeSettings.StandartAnimationDelay;
            var waitDelay = new WaitForSeconds(standartDelay);
            var infinityAnimations = new List<Coroutine>();

            while (animationsQueue.Count > 0)
            {
                Coroutine current = null;
                var animation = animationsQueue.Dequeue();

                if (!(animation is IParallelAnimation || animation is IUserInputAnimation))
                    yield return waitDelay;

                animation.Start?.Invoke();

                if (animation is IParallelAnimation)
                    current = StartCoroutine(animation.Work());

                if (animation is IInfinityAnimation)
                {
                    infinityAnimations.Add(current);
                    (animation as IInfinityAnimation).isInterrupted += () =>
                    {
                        StopCoroutine(current);
                        infinityAnimations.Remove(current);
                    };
                }

                if (current == null)
                {
                    yield return animation.Work();
                    animation.End?.Invoke();
                }
            }

            var waitEndOfFrame = new WaitForEndOfFrame();
            while (infinityAnimations.Count > 0)
                yield return waitEndOfFrame;

            isPlaying = false;
            allAnimationsEnds?.Invoke();

            this.Log("Animator Stop working");
        }
    }
}