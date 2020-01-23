namespace FSM
{
    public class StateFSM<T> : AbsStateFSM where T : IStateFSM<T>, new()
    {
        public T state { get; protected set; }

        public override string ComponentName => typeof(T).FullName;

        public StateFSM()
        {
            state = new T();
        }

        public StateFSM(T state)
        {
            this.state = state;
        }

        public override void Enter()
        {
            base.Enter();
            Debug("is Enter");
            state.Enter();
        }

        public override void Exit()
        {
            Debug("is Exit");
            state.Exit();
        }

        private void Debug(string postfix)
        {
            this.Log($"[StateChanged] {state.StateName} {postfix}");
        }
    }
}