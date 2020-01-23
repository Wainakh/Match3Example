using UnityEngine;

namespace M3T.Animations
{
    public class SwipeTwoChipsFailureAnimation : MoveTwoChipsAnimation
    {
        public SwipeTwoChipsFailureAnimation(Transform tile1, Transform tile2)
            : base(tile1, tile2, limitByBounds: true)
        {

        }
    }
}