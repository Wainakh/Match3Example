namespace FSM
{
    public interface IStateFSM<T> where T : new()
    {
        StatesEnum State { get; }
        string StateName { get; }

        void Enter();
        void Exit();
    }
}