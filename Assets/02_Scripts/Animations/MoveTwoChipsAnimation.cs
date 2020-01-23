using System;
using System.Collections;
using UnityEngine;

namespace M3T.Animations
{
    public class MoveTwoChipsAnimation : Animation, IUserInputAnimation
    {
        Action checkPoint;
        Transform tile1;
        Transform tile2;
        bool limitByBounds = false;

        public MoveTwoChipsAnimation(Transform tile1, Transform tile2, 
            Action checkPoint = null, bool limitByBounds = false)
        {
            this.tile1 = tile1;
            this.tile2 = tile2;
            this.checkPoint = checkPoint;
            this.limitByBounds = limitByBounds;
        }

        public override IEnumerator Work()
        {
            if (Math.Abs(duration) < Mathf.Epsilon)
            {
                checkPoint?.Invoke();
                yield return new WaitForEndOfFrame();
            }
            else
            {
                var halfDuration = new WaitForSeconds(duration * .5f);
                var direction = tile2.localPosition - tile1.localPosition;

                if (limitByBounds)
                {
                    var boundsSize = M3T.GameController.Instance.Board.Generator.GetTileBoundsSize;
                    direction = direction.normalized * Mathf.Min(boundsSize.x, boundsSize.y) * .5f;
                }
                else
                    direction *= .5f;

                var chip1 = coroutineRoot.StartCoroutine(MoveAndComeBackAnimation(tile1, direction, duration));
                var chip2 = coroutineRoot.StartCoroutine(MoveAndComeBackAnimation(tile2, -direction, duration));

                yield return halfDuration;
                checkPoint?.Invoke();
                yield return halfDuration;

                yield return new WaitForEndOfFrame();
                yield return chip1;
                yield return chip2;
            }
        }

        IEnumerator MoveAndComeBackAnimation(Transform tile, Vector3 direction, float currentDuration)
        {
            yield return MoveTransformToDirectionWithFixedDurationAnimation(tile, direction, currentDuration * .5f);
            yield return MoveTransformToDirectionWithFixedDurationAnimation(tile, -direction, currentDuration * .5f);
        }
    }
}