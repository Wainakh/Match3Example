using System;

namespace M3T.Animations
{
    public interface IParallelAnimation
    {
        Action Subscribe { get; set; }
        Action Unsubscribe { get; set; }
    }
}