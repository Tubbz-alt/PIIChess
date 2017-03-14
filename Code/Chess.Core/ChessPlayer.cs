using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class ChessPlayer : IEqualityComparer<ChessPlayer>
    {

        public string NickName { get; set; }
        public ChessColor pieceColor { get; set; }
        public LinkedList<ChessMove> Moves { get; private set; }
        public List<ChessPiece> MyPiecesInBoard { get; set; }//Set Private
        public ChessGame Game { get; private set; }
        public ChessBoardClock Clock { get; set; }
        public Guid Id { get; private set; }
        public event EventHandler<OnMoveRequestedEventArgs> OnMoveRequested;
       // public event EventHandler<OnResignGameEventArgs> OnResignGame;

        public ChessPlayer(string nickName, ChessColor color, ChessGame game, ChessBoardClock clock)
        {
            NickName = nickName;
            pieceColor = color;
            Moves = new LinkedList<ChessMove>();
            MyPiecesInBoard = new List<ChessPiece>();
            game.OnTurnStart += Game_OnTurnStart;
            game.Board.OnNewPieceInserted += Board_OnNewPieceInserted;
            game.Board.OnNewPieceCaptured += Board_OnNewPieceCaptured;
            Game = game;
            Clock = clock;
            Clock.Player = this;
            Id = Guid.NewGuid();
        }



        #region BOARD EVENTS
        private void Board_OnNewPieceInserted(object sender, PieceInsertedEventArgs e)
        {
            if (e.PieceInserted != null && this.Equals(e.PieceInserted.Player))
            {
                if (!this.MyPiecesInBoard.Contains(e.PieceInserted))
                {
                    this.MyPiecesInBoard.Add(e.PieceInserted);
                }
            }
        }

        private void Board_OnNewPieceCaptured(object sender, PieceCapturedEventArgs e)
        {
            if (e.PieceCaptured != null && this.Equals(e.PieceCaptured.Player))
            {
                if (this.MyPiecesInBoard.Contains(e.PieceCaptured))
                {
                    this.MyPiecesInBoard.Remove(e.PieceCaptured);
                }
                else {
                    Console.Write("AAAAA");
                }
            }
        }


        #endregion

        #region TURN MANAGEMENT
        public bool IsMyTurn {
            get {
                return Clock.LastTurnStartAt.HasValue;
            }
        }
        private void Game_OnTurnStart(object sender, OnTurnStartEventArgs e)
        {
            if (this.Equals(e.Player))
            {
                Clock.Start();
            }
        }

        public void FinishTurn()
        {
            Clock.Stop();
        }

        #endregion


        #region PLAYER OPERATIONS

        public void MovePiece(string pgnMove)
        {
            ChessMove move = ChessPGN.GetMoveFromPGN(pgnMove, this, Game.Board);
            RequestMovement(move);
        }

        public void MovePiece(ChessPiece piece, ChessBoardCell toCell)
        {
            ChessMove move = ChessMove.CreateStandardChessMove(piece, toCell);
            RequestMovement(move);
        }

        internal void MovePiece(string from, string to)
        {
            ChessBoardCell fromCell = Game.Board[from];
            ChessBoardCell toCell = Game.Board[to];
            if (fromCell.PieceInCell != null)
            {
                if (this.Equals(fromCell.PieceInCell.Player))
                {
                    ChessMove move = ChessMove.CreateStandardChessMove(fromCell, toCell);
                    RequestMovement(move);
                } else {
                    throw new ChessExceptionInvalidGameAction(ExceptionCode.AttemptToMoveOtherPlayerPiece, "Piece does not belong to this player: " +
                        fromCell.PieceInCell.ToString() +
                        fromCell.ToString() +
                        "["
                        +
                            (fromCell.PieceInCell.Color == ChessColor.ChessColor_Black ? "B" : "W")
                        +
                        "]");
                }

            }
            else {
                throw new ChessExceptionInvalidGameAction(ExceptionCode.InvalidMovement, "Cell is empty: " + from);
            }

        }

        private void RequestMovement(ChessMove move)
        {
            ChessPiece piece = move.Origin.PieceInCell;

            if (this.Equals(piece.Player))
            {

                OnMoveRequested?.Invoke(this, new OnMoveRequestedEventArgs(move));

                if (move.MoveAccepted)
                {
                    FinishTurn();
                }
                else
                {
                    throw new ChessExceptionInvalidGameAction(ExceptionCode.InvalidMovement, "Move: " + move.ToString());
                }
            }
            else
            {
                throw new ChessExceptionInvalidGameAction(ExceptionCode.AttemptToMoveOtherPlayerPiece, "Move: " + move.ToString());
            }
        }

        public void ResignGame()
        {

        }

        public void PauseGame()
        {

        }
        #endregion

        #region BASE OVERRIDES
        public override bool Equals(object obj)
        {
            return this.Id.Equals(((ChessPlayer)obj).Id);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return NickName + (pieceColor == ChessColor.ChessColor_Black ? ":[B]" : ":[W]");
        }

        public bool Equals(ChessPlayer x, ChessPlayer y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(ChessPlayer obj)
        {
            return base.GetHashCode();
        }

        #endregion

    }

    public class OnMoveRequestedEventArgs:EventArgs
    {
        public ChessMove Move { get; set; }
        public OnMoveRequestedEventArgs(ChessMove move)
        {
            Move = move;
        }
    }

    public class OnResignGameEventArgs : EventArgs
    {
        public OnResignGameEventArgs()
        { }
    }

}
