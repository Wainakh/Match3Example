using System;
using UnityEngine;

namespace M3T.Animations
{
    public class SwipeTwoChipsSuccessAnimation : MoveTwoChipsAnimation
    {
        public SwipeTwoChipsSuccessAnimation(Transform tile1, Transform tile2, Action CheckPoint)
            : base(tile1, tile2, CheckPoint, limitByBounds: false)
        {

        }
    }
}