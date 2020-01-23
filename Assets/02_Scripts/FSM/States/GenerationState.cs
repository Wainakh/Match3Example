
namespace FSM
{
    public abstract class GenerationState : AbsStateFSM
    {
        protected int tryCount = 0;

        protected bool ReGenerateBoard()
        {
            tryCount++;
            gameController.Board.CreateBoard();

            var matches = gameController.Matcher.FoundMatches(gameController.Board.Tiles);
            if (matches != null && matches.Count > 0)
                ReGenerateBoard();

            var predictions = gameController.Matcher.PredictMatches(gameController.Board.Tiles);
            if (predictions == null || predictions.Count == 0)
                ReGenerateBoard();

            return true;
        }
    }
}