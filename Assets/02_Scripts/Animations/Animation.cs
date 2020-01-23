using System;
using System.Collections;
using UnityEngine;

namespace M3T.Animations
{
    /// <summary>
    /// Base class for programming animations.
    /// </summary>
    public abstract class Animation : IDebugComponent
    {
        public MonoBehaviour coroutineRoot;
        public Action Start;
        public Action End;
        public float duration;
        public string ComponentName => typeof(Animation).FullName;
        public abstract IEnumerator Work();

        /// <summary>
        /// Base implementation movement transform to direction with fixed duration
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="direction"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        protected IEnumerator MoveTransformToDirectionWithFixedDurationAnimation(Transform transform, Vector3 direction, float duration)
        {
            var from = transform.localPosition;
            if (duration > 0)
            {
                var endTimeStamp = Time.realtimeSinceStartup + duration;
                var waitEndOfFrame = new WaitForEndOfFrame();
                transform.localPosition += Vector3.back;
                var currentDirection = direction + Vector3.back;
                var koeff = Mathf.Abs(currentDirection.magnitude / duration);
                currentDirection.Normalize();
                while (Time.realtimeSinceStartup < endTimeStamp)
                {
                    transform.localPosition += currentDirection * (koeff * Time.deltaTime);
                    yield return waitEndOfFrame;
                }
            }
            transform.localPosition = from + direction;
        }
    }
}