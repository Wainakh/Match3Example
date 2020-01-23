using M3T;
using M3T.Animations;

namespace FSM
{
    public class InitializeBoardState : GenerationState, IStateFSM<InitializeBoardState>
    {
        public StatesEnum State => StatesEnum.Initialization;
        public string StateName => State.ToString();
        public override string ComponentName => typeof(InitializeBoardState).FullName;

        bool firstRun = true;

        public override void Enter()
        {
            base.Enter();

            if (firstRun)
            {
                gameController.Board.onSelectedTwoChips -= gameController.OnSelectedTwoChips;
                gameController.Board.onUserSelectChip -= gameController.OnUserSelectChip;
                gameController.Board.onSelectedTwoChips += gameController.OnSelectedTwoChips;
                gameController.Board.onUserSelectChip += gameController.OnUserSelectChip;
            }
            GenerateBoard();
            this.Log("End initialization. Tryed [" + tryCount + "] to create board");
            firstRun = false;
            CreateFallingAnimation(() => gameController.ControllerFSM.SetState(StatesEnum.WaitUserInput));
        }

        void GenerateBoard()
        {
            ReGenerateBoard();
        }

        public override void Exit() { }

        public void CreateFallingAnimation(System.Action callback)
        {
            var gameController = GameController.Instance;
            gameController.Animator.AddAnimation(new AnimationFallingFixSpeed(gameController.Board.TilesAsListOfLines, 
                fallingSpeed: gameController.AnimationsTimeSettings.FallingSpeed)
            {
                duration = gameController.AnimationsTimeSettings.FallingDelayBetweenLines,
                Start = () => { this.Log("[InitializeBoardState] Start anim falling"); },
                End = callback,
            });
        }
    }
}