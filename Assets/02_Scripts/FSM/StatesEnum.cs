namespace FSM
{
    [System.Flags]
    public enum StatesEnum : byte
    {
        NONE = 0,

        WaitUserInput = 1,
        EnterUserInput = 2,

        Initialization = 4,
        FindMatches = 8,
        HaveNoOneTurn = 16,

        AvailableUserInput = WaitUserInput | EnterUserInput,
    }
}