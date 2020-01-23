using M3T;

namespace FSM
{
    public abstract class AbsStateFSM : IDebugComponent
    {
        protected GameController gameController;

        public abstract string ComponentName { get; }

        public virtual void Enter()
        {
            gameController = GameController.Instance;
        }

        public abstract void Exit();
    }
}