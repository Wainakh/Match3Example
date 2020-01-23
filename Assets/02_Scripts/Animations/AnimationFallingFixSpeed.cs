using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M3T.Animations
{
    public class AnimationFallingFixSpeed : Animation
    {
        List<List<Tile>> matches;
        float fallingSpeed;

        public AnimationFallingFixSpeed(List<List<Tile>> matches, float fallingSpeed = .02f)
        {
            this.matches = matches;
            this.fallingSpeed = fallingSpeed;
        }

        public override IEnumerator Work()
        {
            var waitFallingDelayBetweenLines = new WaitForSeconds(duration);
            var coroutines = new List<Coroutine>();
            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (match.IsNullOrDefault() || match.Count == 0)
                    continue;
                for (int k = 0; k < match.Count; k++)
                {
                    var tile = match[k];
                    if ((tile.transform.localPosition - tile.DefaultPosition).magnitude > Mathf.Epsilon)
                        coroutines.Add(coroutineRoot.StartCoroutine(Falling(tile, duration)));
                }
                yield return waitFallingDelayBetweenLines;
            }

            for (int i = 0; i < coroutines.Count; i++)
                yield return coroutines[i];

            yield return null;
        }

        IEnumerator Falling(Tile tile, float duration)
        {
            var currentPosition = tile.transform.localPosition;
            var targetPosition = tile.DefaultPosition;
            if (duration > 0)
            {
                var waitEndFrame = new WaitForEndOfFrame();
                var direction = targetPosition - tile.transform.localPosition;
                direction.Normalize();
                while (Vector3.Distance(targetPosition, tile.transform.localPosition) > fallingSpeed)
                {
                    tile.transform.localPosition += direction * fallingSpeed;
                    yield return waitEndFrame;
                }
            }
            tile.transform.localPosition = targetPosition;
        }
    }
}