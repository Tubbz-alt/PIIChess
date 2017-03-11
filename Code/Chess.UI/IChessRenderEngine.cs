using System;

namespace TestAsync.Chess.UI
{
    public interface IChessRenderEngine
    {
        event EventHandler OnAsyncMessageWasShown;

        void RenderGameBoard(ChessGame game);
        void RenderGameStatus(ChessGame game);
        void RenderHelp();
        void RenderWelcomeMessage();
        void ShowMessage(string message);
        void ShowError(string message);
        void ShowAlert(string message);
        void PromptCommand(ChessGame game);
        void RenderCurrentPlayerTime(ChessGame game);
        void Clear();
        void Debug(ChessGame game);
    }
}