using System;

namespace M3T.Animations
{
    public interface IInfinityAnimation
    {
        event Action isInterrupted;
        void Interrupt();
    }
}