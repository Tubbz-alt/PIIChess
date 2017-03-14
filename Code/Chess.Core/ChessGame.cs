using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{    
    /// <summary>
    /// This class manages the game, it ensures that the chess rules will be observed.
    /// </summary>
    public class ChessGame
    {
        public event EventHandler OnGameStart;
        public event EventHandler<OnTurnStartEventArgs> OnTurnStart;

        public ChessPlayer PlayerWhite { get; private set; }
        public ChessPlayer PlayerBlack { get; private set; }
        public ChessPlayer NextTurnPlayer { get; private set; }
        public ChessBoard Board { get; private set; }
        public string GameName { get; set; }
        public string GameLocation { get; set; }
        public DateTime StartDateUTC { get; private set; }
        public DateTime EndDateUTC { get; private set; }
        public int Round { get; private set; }
        public ChessGameStatus GameStatus {get; private set; }
        private readonly TimeSpan defaultTimeAvailablePerPlayer;
        public ChessGame()
        {
            Board = new ChessBoard();
            GameStatus = ChessGameStatus.ChessGameStatus_NonStarted;
            defaultTimeAvailablePerPlayer = new TimeSpan(0, 15, 0);
        }


        public void PrepareGame(string blackPlayerName, string whitePlayerName, string gameName)
        {
            if (GameStatus == ChessGameStatus.ChessGameStatus_NonStarted ||
                GameStatus == ChessGameStatus.ChessGameStatus_Finihed
                )
            {
                ChessBoardClock PlayerWhiteClock = new ChessBoardClock();
                ChessBoardClock PlayerBlackClock = new ChessBoardClock();
                PlayerWhiteClock.InitClock(defaultTimeAvailablePerPlayer);
                PlayerBlackClock.InitClock(defaultTimeAvailablePerPlayer);

                PlayerBlackClock.OnClockStop += PlayerClock_OnClockStop;
                PlayerBlackClock.OnTimeFinish += PlayerClock_OnTimeFinish;

                PlayerWhiteClock.OnClockStop += PlayerClock_OnClockStop;
                PlayerWhiteClock.OnTimeFinish += PlayerClock_OnTimeFinish;

                PlayerWhite = new ChessPlayer(whitePlayerName, ChessColor.ChessColor_White, this, PlayerWhiteClock);
                PlayerBlack = new ChessPlayer(blackPlayerName, ChessColor.ChessColor_Black, this, PlayerBlackClock);

                PlayerWhite.OnMoveRequested += Player_OnMoveRequested;
                PlayerBlack.OnMoveRequested += Player_OnMoveRequested;

                InitializeBoard();

                GameName = gameName;
            }else{
                throw new ChessExceptionInvalidGameAction(ExceptionCode.AnotherGameInProgress, "Cannot prepare a new game, another game is in progress");
            }
        }



        public void StartNewGame()
        {
           if(GameStatus == ChessGameStatus.ChessGameStatus_NonStarted ||
              GameStatus == ChessGameStatus.ChessGameStatus_Finihed)
           {
                StartDateUTC = DateTime.UtcNow;                
                
                StartTurnForPlayer(PlayerWhite);
                GameStatus = ChessGameStatus.ChessGameStatus_Playing;
                Round = 0;
                OnGameStartEventRaiser();
            }else{
                throw new ChessExceptionInvalidGameAction(ExceptionCode.AnotherGameInProgress, "Cannot start a new game, another game is in progress");
            }

        }
        public void ResignGane()
        {
            if(GameStatus == ChessGameStatus.ChessGameStatus_Playing)
            {
                EndDateUTC = DateTime.UtcNow;
                GameStatus = ChessGameStatus.ChessGameStatus_Finihed;
            }            
        }


        public void OnGameStartEventRaiser()
        {            
            OnGameStart?.Invoke(this, new EventArgs());
        }
        private void StartTurnForPlayer(ChessPlayer player)
        {
            NextTurnPlayer = player;
            OnTurnStart?.Invoke(this, new OnTurnStartEventArgs(NextTurnPlayer));
        }


        private void InitializeBoard()
        {
            Task armarBlancas = new Task(() =>
            {
                ChessPiece peon1 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerWhite);
                ChessPiece peon2 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerWhite);
                ChessPiece peon3 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerWhite);
                ChessPiece peon4 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerWhite);
                ChessPiece peon5 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerWhite);
                ChessPiece peon6 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerWhite);
                ChessPiece peon7 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerWhite);
                ChessPiece peon8 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerWhite);

                ChessPiece torre1 = new ChessPiece(ChessPieceType.ChessPieceType_Rook, PlayerWhite);
                ChessPiece torre2 = new ChessPiece(ChessPieceType.ChessPieceType_Rook, PlayerWhite);
                ChessPiece caballo1 = new ChessPiece(ChessPieceType.ChessPieceType_Knight, PlayerWhite);
                ChessPiece caballo2 = new ChessPiece(ChessPieceType.ChessPieceType_Knight, PlayerWhite);
                ChessPiece alfil1 = new ChessPiece(ChessPieceType.ChessPieceType_Bishop, PlayerWhite);
                ChessPiece alfil2 = new ChessPiece(ChessPieceType.ChessPieceType_Bishop, PlayerWhite);
                ChessPiece reina = new ChessPiece(ChessPieceType.ChessPieceType_Queen, PlayerWhite);
                ChessPiece rey = new ChessPiece(ChessPieceType.ChessPieceType_King, PlayerWhite);


                Board.InsertPieceInBoard(torre1, "a1");
                Board.InsertPieceInBoard(torre2, "h1");
                Board.InsertPieceInBoard(caballo1, "b1");
                Board.InsertPieceInBoard(caballo2, "g1");
                Board.InsertPieceInBoard(alfil1, "c1");
                Board.InsertPieceInBoard(alfil2, "f1");
                Board.InsertPieceInBoard(reina, "d1");
                Board.InsertPieceInBoard(rey, "e1");

                Board.InsertPieceInBoard(peon1, "a2");
                Board.InsertPieceInBoard(peon2, "b2");
                Board.InsertPieceInBoard(peon3, "c2");
                Board.InsertPieceInBoard(peon4, "d2");
                Board.InsertPieceInBoard(peon5, "e2");
                Board.InsertPieceInBoard(peon6, "f2");
                Board.InsertPieceInBoard(peon7, "g2");
                Board.InsertPieceInBoard(peon8, "h2");

            });
            Task armarNegras = new Task(() =>
            {

                ChessPiece peon1 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerBlack);
                ChessPiece peon2 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerBlack);
                ChessPiece peon3 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerBlack);
                ChessPiece peon4 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerBlack);
                ChessPiece peon5 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerBlack);
                ChessPiece peon6 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerBlack);
                ChessPiece peon7 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerBlack);
                ChessPiece peon8 = new ChessPiece(ChessPieceType.ChessPieceType_Pawn, PlayerBlack);

                ChessPiece torre1 = new ChessPiece(ChessPieceType.ChessPieceType_Rook, PlayerBlack);
                ChessPiece torre2 = new ChessPiece(ChessPieceType.ChessPieceType_Rook, PlayerBlack);
                ChessPiece caballo1 = new ChessPiece(ChessPieceType.ChessPieceType_Knight, PlayerBlack);
                ChessPiece caballo2 = new ChessPiece(ChessPieceType.ChessPieceType_Knight, PlayerBlack);
                ChessPiece alfil1 = new ChessPiece(ChessPieceType.ChessPieceType_Bishop, PlayerBlack);
                ChessPiece alfil2 = new ChessPiece(ChessPieceType.ChessPieceType_Bishop, PlayerBlack);
                ChessPiece reina = new ChessPiece(ChessPieceType.ChessPieceType_Queen, PlayerBlack);
                ChessPiece rey = new ChessPiece(ChessPieceType.ChessPieceType_King, PlayerBlack);



                Board.InsertPieceInBoard(torre1, "a8");
                Board.InsertPieceInBoard(torre2, "h8");
                Board.InsertPieceInBoard(caballo1, "b8");
                Board.InsertPieceInBoard(caballo2, "g8");
                Board.InsertPieceInBoard(alfil1, "c8");
                Board.InsertPieceInBoard(alfil2, "f8");
                Board.InsertPieceInBoard(reina, "d8");
                Board.InsertPieceInBoard(rey, "e8");


                Board.InsertPieceInBoard(peon1, "a7");
                Board.InsertPieceInBoard(peon2, "b7");
                Board.InsertPieceInBoard(peon3, "c7");
                Board.InsertPieceInBoard(peon4, "d7");
                Board.InsertPieceInBoard(peon5, "e7");
                Board.InsertPieceInBoard(peon6, "f7");
                Board.InsertPieceInBoard(peon7, "g7");
                Board.InsertPieceInBoard(peon8, "h7");

            });

            armarBlancas.Start();
            armarNegras.Start();

            armarBlancas.Wait();
            armarNegras.Wait();


        }
        public string  GameStatusAsString()
        {
             switch(GameStatus)
            {
                case ChessGameStatus.ChessGameStatus_Finihed: return "Finished";
                case ChessGameStatus.ChessGameStatus_NonStarted: return "No Started";
                case ChessGameStatus.ChessGameStatus_Paused: return "Paused";
                case ChessGameStatus.ChessGameStatus_Playing: return "Playing";
                case ChessGameStatus.ChessGameStatus_Unknown: return "Unknown";
                default: return "";
            }

        }

        #region CLOCKS EVENTS
        //The game never stop the clock itself, but it starts the clocks automatically.
        private void PlayerClock_OnTimeFinish(object sender, ChessClockEventArgs e)
        {
            GameStatus = ChessGameStatus.ChessGameStatus_Finihed;
        }

        private void PlayerClock_OnClockStop(object sender, ChessClockEventArgs e)
        {
            //The player has stoped his clock.
            ChessPlayer p = e.Player; 
            if (p.Equals(PlayerBlack))
            {
                StartTurnForPlayer(PlayerWhite);
            }
            else {
                StartTurnForPlayer(PlayerBlack);
            }

        }
        #endregion

        #region PLAYER EVENTS
        private void Player_OnMoveRequested(object sender, OnMoveRequestedEventArgs e)
        {
            ChessMove move = e.Move;
            try
            {               
                //CHECK IF THE MOVEMENT IS VALID FOR THIS PIECE
                //CHECK IF THE MOVEMENT DOES NOT PUT THE OWN KING IN RISK
                //
                if (e.Move.Destiny.PieceInCell != null)
                {
                    //Atttempt to move to an occupied cell.
                    if (!move.PieceToMove.Player.Equals(move.Destiny.PieceInCell.Player))
                    {
                        //isCapture                    

                        Board.MovePiece(move.PieceToMove, move.Destiny.RowIndex, move.Destiny.ColumnIndex);
                        move.MoveAccepted = true;
                    }
                    else
                    {
                        //Invalid Movement.
                        move.MoveAccepted = false;
                    }
                }
                else
                {
                    //Move to an empty cell
                    Board.MovePiece(move.PieceToMove, move.Destiny.RowIndex, move.Destiny.ColumnIndex);
                    move.MoveAccepted = true;
                }
            }
            catch (ChessExceptionInvalidPGNNotation)
            {
                move.MoveAccepted = false;
            }
            catch (ChessExceptionInvalidGameAction)
            {
                move.MoveAccepted = false;
            }
            catch (ChessException)
            {
                move.MoveAccepted = false;
            }
            catch (Exception)
            {
                move.MoveAccepted = false;
            }
            

          
        }
        #endregion
    }

    public class OnTurnStartEventArgs : EventArgs
    {
        public ChessPlayer Player { get; set; }
        public OnTurnStartEventArgs(ChessPlayer player)
        {
            Player = player;
        }
    }

}
