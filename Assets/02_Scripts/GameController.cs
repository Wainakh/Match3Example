using FSM;
using M3T.Animations;

namespace M3T
{
    /// <summary>
    /// Экземпляр текущей игры. Имеет ссылки на все классы.
    /// </summary>
    public class GameController : Singleton<GameController>
    {
        #region Временный костыль ввиду отсутсвия UserInput
        internal void OnTileClick(Tile tile)
        {
            //var res = controllerFSM.CurrentState == StatesEnum.WaitUserInput ||
            //controllerFSM.CurrentState == StatesEnum.EnterUserInput;
            var res = (ControllerFSM.CurrentState & StatesEnum.AvailableUserInput) != 0;
            if (res) Board.OnTileClick(tile);
        }
        #endregion

        public Matcher Matcher { get; private set; }
        public TileAnimator Animator { get; private set; }
        public IBoard Board { get; private set; }
        public ControllerFSM ControllerFSM { get; private set; }
        public AnimationsTimeSettings AnimationsTimeSettings { get; private set; }

        private void Start()
        {
            Matcher = GetComponent<Matcher>();
            Animator = GetComponent<TileAnimator>();
            Board = GetComponent<Board>();
            ControllerFSM = GetComponent<ControllerFSM>();
            AnimationsTimeSettings = GetComponent<AnimationsTimeSettings>();

            ControllerFSM.RegisterNewState(StatesEnum.Initialization, new StateFSM<InitializeBoardState>());
            ControllerFSM.RegisterNewState(StatesEnum.WaitUserInput, new StateFSM<WaitUserInputState>());
            ControllerFSM.RegisterNewState(StatesEnum.EnterUserInput, new StateFSM<CheckUserInputState>());
            ControllerFSM.RegisterNewState(StatesEnum.FindMatches, new StateFSM<CheckMatchesState>());
            ControllerFSM.RegisterNewState(StatesEnum.HaveNoOneTurn, new StateFSM<HaveNoOneTurnState>());

            ControllerFSM.SetState(StatesEnum.Initialization);
        }

        public void OnSelectedTwoChips(Tile tile1, Tile tile2) { }

        public void OnUserSelectChip()
        {
            if (ControllerFSM.CurrentState == StatesEnum.WaitUserInput)
                ControllerFSM.SetState(StatesEnum.EnterUserInput);
        }
    }
}
