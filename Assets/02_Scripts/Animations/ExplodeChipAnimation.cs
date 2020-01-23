using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M3T.Animations
{
    public class ExplodeChipAnimation : Animation
    {
        List<List<Tile>> matches;

        public ExplodeChipAnimation(List<List<Tile>> matches)
        {
            this.matches = matches;
        }

        public override IEnumerator Work()
        {
            var waitForSeconds = new WaitForSeconds(duration);
            for (int i = 0; i < matches.Count; i++)
                for (int k = 0; k < matches[i].Count; k++)
                {
                    matches[i][k].Explode();
                    yield return waitForSeconds;
                }
        }
    }
}