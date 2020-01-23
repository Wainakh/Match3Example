using M3T;
using M3T.Animations;

namespace FSM
{
    public class CheckUserInputState : AbsStateFSM, IStateFSM<CheckUserInputState>
    {
        public StatesEnum State => StatesEnum.EnterUserInput;
        public string StateName => State.ToString();
        public override string ComponentName => typeof(CheckUserInputState).FullName;

        public override void Enter()
        {
            base.Enter();
            gameController.Board.onSelectedTwoChips += CheckUserInput;
        }

        public override void Exit()
        {
            gameController.Board.onSelectedTwoChips -= CheckUserInput;
        }

        void CheckUserInput(Tile tile1, Tile tile2)
        {
            gameController.Board.onSelectedTwoChips -= CheckUserInput;

            if (tile1.IsNullOrDefault() || tile2.IsNullOrDefault())
            {
                ChangeStateByFailure();
                return;
            }

            var matches = gameController.Matcher.FindMatchesIfSwipeSuccess(gameController.Board.Tiles, tile1, tile2);

            if (!matches.IsNullOrDefault() && matches.Count > 0)
                CreateSuccessAnimation(tile1, tile2, ChangeStateBySuccess);
            else
                CreateFailureAnimation(tile1, tile2, ChangeStateByFailure);
        }

        void ChangeStateByFailure()
        {
            this.Log("End anim failure swipe");
            gameController.Board.ResetAllSelectedTiles();
            gameController.ControllerFSM.SetState(StatesEnum.WaitUserInput);
        }

        void ChangeStateBySuccess()
        {
            this.Log("End anim success swipe");
            gameController.Board.ResetAllSelectedTiles();
            gameController.ControllerFSM.SetState(StatesEnum.FindMatches);
        }

        void CreateFailureAnimation(Tile tile1, Tile tile2, System.Action callback)
        {
            gameController.Animator.AddAnimation(new SwipeTwoChipsFailureAnimation(tile1.transform, tile2.transform)
            {
                duration = gameController.AnimationsTimeSettings.SwipeMoveDuration,
                Start = () => { this.Log("Start anim failure swipe"); },
                End = callback,
            });
        }

        void CreateSuccessAnimation(Tile tile1, Tile tile2, System.Action callback)
        {
            gameController.Animator.AddAnimation(new SwipeTwoChipsSuccessAnimation(tile1.transform, tile2.transform,
               CheckPoint: () => { gameController.Matcher.SwipeChipsSilent(tile1, tile2); })
            {
                duration = gameController.AnimationsTimeSettings.SwipeMoveDuration,
                Start = () => { this.Log("Start anim success swipe"); },
                End = callback,
            });
        }
    }
}