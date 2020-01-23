using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M3T.Animations
{
    public class AnimationFallingFixDuration : Animation
    {
        FallingAnimationType animType = FallingAnimationType.sameTime;
        List<List<Tile>> matches;

        public AnimationFallingFixDuration(List<List<Tile>> matches, FallingAnimationType animType = FallingAnimationType.sameTime)
        {
            this.matches = matches;
            this.animType = animType;
        }

        IEnumerator Falling(Tile tile, float duration)
        {
            var from = tile.transform.localPosition;
            var to = tile.DefaultPosition;
            if (from == to)
                yield break;
            if (duration > 0)
            {
                var endTimeStamp = Time.realtimeSinceStartup + duration;
                var waitForEndOfFrame = new WaitForEndOfFrame();
                tile.transform.localPosition += Vector3.back;
                var direction = (to + Vector3.back) - tile.transform.localPosition;
                var koeff = Mathf.Abs(direction.magnitude / duration);
                direction.Normalize();
                while (Time.realtimeSinceStartup < endTimeStamp)
                {
                    tile.transform.localPosition += direction * (koeff * Time.deltaTime);
                    yield return waitForEndOfFrame;
                }
            }
            tile.transform.localPosition = to;
        }

        public override IEnumerator Work()
        {
            var waitForSeconds = new WaitForSeconds(duration);
            for (int i = 0; i < matches.Count; i++)
            {
                for (int k = 0; k < matches[i].Count; k++)
                {
                    var tile = matches[i][k];
                    if (tile.transform.localPosition != tile.DefaultPosition)
                    {
                        coroutineRoot.StartCoroutine(Falling(tile, duration));
                        if (animType == FallingAnimationType.perOne)
                            yield return waitForSeconds;
                    }
                }
                if (animType == FallingAnimationType.byLine)
                    yield return waitForSeconds;
            }
        }
    }
}
