using Chess.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.UI
{
    
    public class ConsoleRenderEngine : IChessRenderEngine
    {
        
        private const ConsoleColor PromptColor = ConsoleColor.White;
        private const ConsoleColor MessageColor = ConsoleColor.Yellow;

        private const ConsoleColor ErrorForegroundColor = ConsoleColor.Red;
        private const ConsoleColor BlackPieceColor = ConsoleColor.Blue;
        private const ConsoleColor WhitePieceColor = ConsoleColor.Yellow;
        private const ConsoleColor WhiteCellColor = ConsoleColor.Gray;
        private const ConsoleColor BlackCellColor = ConsoleColor.Black;
        private const ConsoleColor BoardBackgroundColor = ConsoleColor.DarkRed;
        private const ConsoleColor BoardForegroundColor = ConsoleColor.White;
        private const ConsoleColor BoardSpecialBackgroundColor = ConsoleColor.DarkGray;

        private const ConsoleColor AlertForeColor = ConsoleColor.Black;
        private const ConsoleColor AlertBackgroundColor = ConsoleColor.Yellow;

        public event EventHandler OnAsyncMessageWasShown;

        public ConsoleRenderEngine()
        {
            Console.ForegroundColor = PromptColor;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.Title = "PII CHESS";
            Console.ResetColor();
        }

        public void RenderWelcomeMessage()
        {
            Console.WriteLine("PII CHESS          THE WORST CHESS GAME IN THE UNIVERSE");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("                    by Fernando Sosa (fjsosa@gmail.com)");
            Console.WriteLine("                                          version: 0.1 ");
        }
        public void RenderHelp()
        {
            Console.WriteLine("Command List");
            Console.WriteLine(" Create <WhitePlayerName> <BlackPlayerName> <GameName>");            
            Console.WriteLine(" Move <FROM> <TO>");
            Console.WriteLine(" Start");
            Console.WriteLine(" Status");
            Console.WriteLine(" Help");
            Console.WriteLine(" Help <COMMAND>");
        }
        public void RenderGameStatus(ChessGame game)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" Game: " + game.GameName + "\t\tStatus: " + game.GameStatusAsString());
            sb.AppendLine(" Player 1: " + game.PlayerWhite.ToString() + (game.PlayerWhite.IsMyTurn ? "-" : ""));
            sb.AppendLine(" Player 2: " + game.PlayerBlack.ToString() + (game.PlayerBlack.IsMyTurn ? "-" : ""));
            Console.Write(sb.ToString());
        }
        public void RenderGameBoard(ChessGame game)
        {
            for (int row = 8; row >= 0; row--)
            {
                if (row == 8)
                {
                    Console.BackgroundColor = BoardBackgroundColor;
                    Console.ForegroundColor = BoardForegroundColor;
                    Console.WriteLine("╔══════╦═════╦═════╦═════╦═════╦═════╦═════╦═════╦═════╗");
                    Console.WriteLine("║      ║  a  ║  b  ║  c  ║  d  ║  e  ║  f  ║  g  ║  h  ║");
                    Console.WriteLine("╚══════╩═════╩═════╩═════╩═════╩═════╩═════╩═════╩═════╝");
                    Console.WriteLine("┌──────┬─────┬─────┬─────┬─────┬─────┬─────┬─────┬─────┐");


                }
                else
                {
                    for (int column = -1; column < 8; column++)
                    {
                        Console.BackgroundColor = BoardBackgroundColor;
                        Console.ForegroundColor = BoardForegroundColor;
                        if (column == -1)
                        {
                            Console.Write("│  " + (row + 1).ToString() + "   ");
                        }
                        else
                        {
                            Console.Write("│");
                            ConsoleColor CellBackgroundColor = ConsoleColor.Black;
                            ConsoleColor CellPieceColor = ConsoleColor.White;
                            bool UseSpecialBackgroundColor = false;
                            ChessBoardCell cell = game.Board[row, column];

                            if (cell.PieceInCell != null)
                            {
                                if (cell.PieceInCell.Color == ChessColor.ChessColor_Black)
                                {
                                    CellPieceColor = BlackPieceColor;
                                }
                                else
                                {
                                    CellPieceColor = WhitePieceColor;
                                }
                                UseSpecialBackgroundColor = cell.CellColor == cell.PieceInCell.Color;

                            }

                            if (cell.CellColor == ChessColor.ChessColor_Black)
                            {
                                CellBackgroundColor = BlackCellColor;
                            }
                            else
                            {
                                CellBackgroundColor = WhiteCellColor;
                            }


                            if (cell.PieceInCell != null)
                            {
                                Console.BackgroundColor = CellBackgroundColor;
                                Console.ForegroundColor = CellPieceColor;
                                Console.Write("  ");
                                if (UseSpecialBackgroundColor)
                                {
                                    Console.BackgroundColor = BoardSpecialBackgroundColor;
                                }
                                else
                                {
                                    Console.BackgroundColor = CellBackgroundColor;
                                }
                                Console.Write(cell.PieceInCell.ToString());
                                Console.BackgroundColor = CellBackgroundColor;
                                Console.Write("  ");
                            }
                            else
                            {
                                Console.BackgroundColor = CellBackgroundColor;
                                Console.Write("     ");
                            }


                        }
                    }
                    Console.BackgroundColor = BoardBackgroundColor;
                    Console.ForegroundColor = BoardForegroundColor;
                    Console.WriteLine("│");
                    if (row == 0)
                    {
                        Console.WriteLine("└──────┴─────┴─────┴─────┴─────┴─────┴─────┴─────┴─────┘");
                    }
                    else
                    {
                        Console.WriteLine("├──────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┼─────┤");
                    }
                }
            }

            string wCaptured = GetCapturedPieces(game, ChessColor.ChessColor_White);
            string bCaptured = GetCapturedPieces(game, ChessColor.ChessColor_Black);


            Console.BackgroundColor = BoardBackgroundColor;
            Console.ForegroundColor = BoardForegroundColor;
            Console.WriteLine("┌──────────────────────────────────────────────────────┐");
            Console.WriteLine("│Captured Pieces                                       │");
            Console.WriteLine("└──────────────────────────────────────────────────────┘");
            Console.WriteLine("┌─────┬────────────────────────────────────────────────┐");
            Console.WriteLine("│ [W] │" + wCaptured + "                                │");
            Console.WriteLine("├─────┼────────────────────────────────────────────────┤");
            Console.WriteLine("│ [B] │" + bCaptured + "                                │");
            Console.WriteLine("└──────────────────────────────────────────────────────┘");
            Console.ResetColor();

        }

        private string GetCapturedPieces(ChessGame game, ChessColor color)
        {
            StringBuilder sb = new StringBuilder();
            ChessPiece[] captured = game.Board.CapturedPieces.Where(p => p.Color == color).ToArray();
            sb = new StringBuilder();
            foreach (ChessPiece cap in captured)
            {
                sb.Append(cap.ToString() + " ");
            }
            return (sb.ToString() + new string(' ', 16)).Substring(0, 16);
        }

        public void RenderCurrentPlayerTime(ChessGame game)
        {
            Console.ForegroundColor = MessageColor;
            TimeSpan timeLeft = game.NextTurnPlayer.Clock.LeftTime;
            string msg = string.Format("Avaliable: {0:00}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        public void ShowMessage(string message)
        {
            Console.ForegroundColor = MessageColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void ShowError(string message)
        {
            Console.ForegroundColor = ErrorForegroundColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void ShowAlert(string message)
        {
            Console.ForegroundColor = AlertForeColor;
            Console.BackgroundColor = AlertBackgroundColor;
            Console.WriteLine("\n--> ALERT: " + message + "\n");
            Console.Beep();
            Console.ResetColor();
        }
        public void PromptCommand(ChessGame game)
        {
            Console.ForegroundColor = PromptColor;
            if (game!= null && game.GameStatus == ChessGameStatus.ChessGameStatus_Playing)
            {
                Console.Write("Chess["+game.GameName+"]:"+game.NextTurnPlayer.ToString()+":> ");
            }
            else {
                Console.Write("Chess:> ");
            }
            Console.ResetColor();
            
        }
        public void Clear()
        {
            Console.ResetColor();
            Console.Clear();
        }

        public void Debug(ChessGame game)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\chess.debug.txt";
            FileStream f = File.Open(path, FileMode.Create, FileAccess.Write);

            StringBuilder debugString = new StringBuilder();
            debugString.AppendLine("GAME: " + game.GameName + ", Status: " + game.GameStatus);
            debugString.AppendLine("  NEXT PLAYER: " +  ( game.NextTurnPlayer != null? "": "") );
            debugString.AppendLine("  STARTED: " + (game.StartDateUTC != null ? game.StartDateUTC.ToString() : ""));
            debugString.AppendLine("BOARD");
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    ChessBoardCell cell = game.Board[row, col];
                    debugString.AppendLine("  CELL [" + row + ", " + col + "]: CellColor: " + cell.CellColor + " CellName:" + cell.CellAlgebraicName);
                    if (cell.PieceInCell != null)
                    {
                        debugString.AppendLine("        Piece: " + cell.PieceInCell.ToString());
                        debugString.AppendLine("        Piece Id: " + cell.PieceInCell.PieceId.ToString());
                        debugString.AppendLine("        Piece Initial Position: " + cell.PieceInCell.InitialPosition.ToString());
                        debugString.AppendLine("        Piece Current Position: " + cell.PieceInCell.BoardPosition.ToString());
                        debugString.AppendLine("        Piece Color: " + cell.PieceInCell.Color);
                        debugString.AppendLine("        Piece Owner: " + cell.PieceInCell.Player.ToString());
                    }
                    
                }

            string wCaptured = GetCapturedPieces(game, ChessColor.ChessColor_White);
            string bCaptured = GetCapturedPieces(game, ChessColor.ChessColor_White);
            debugString.AppendLine("White Captured Pieces: " + wCaptured);
            debugString.AppendLine("Black Captured Pieces: " + bCaptured);

            
            PrintDebugDataForPlayer(game.PlayerWhite, debugString);
            PrintDebugDataForPlayer(game.PlayerBlack, debugString);
            string strToWrite = debugString.ToString();
            byte[] bytesToWrite = Encoding.ASCII.GetBytes(strToWrite);
            AsyncCallback callBack = (ar) => {                
                var fs = (FileStream)ar.AsyncState;
                fs.Close();
                ShowAlert("File Created");
                OnAsyncMessageWasShown?.Invoke(this, new EventArgs());
            };
            f.BeginWrite(bytesToWrite, 0, bytesToWrite.Length, callBack, f);
        }

        private void PrintDebugDataForPlayer(ChessPlayer player, StringBuilder debugString)
        {
            debugString.AppendLine("Player: " + player + "[" + player.pieceColor + "]");
            debugString.AppendLine("  ID: " + player.Id.ToString());
            debugString.AppendLine("  IsMyTurn: " + player.IsMyTurn);
            debugString.AppendLine("  Clock: " + player.Clock.LeftTime);
            debugString.AppendLine("  LastTurnStarted: " + player.Clock.LastTurnStartAt);
            foreach (ChessPiece myPiece in player.MyPiecesInBoard)
            {
                debugString.AppendLine("  Piece: " + myPiece.ToString() + myPiece.BoardPosition);
                debugString.AppendLine("     id: " + myPiece.PieceId);
                debugString.AppendLine("  Init P: " + myPiece.InitialPosition.ToString());
            }
        }
    }
      
}
