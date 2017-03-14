using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class ChessPiece: IEqualityComparer<ChessPiece>
    {        

        public double IntrinsicValue {
            get{
                switch (PieceType)
                {
                    case ChessPieceType.ChessPieceType_Pawn:
                        return 1;
                    case ChessPieceType.ChessPieceType_Rook:
                        return 5.5;
                    case ChessPieceType.ChessPieceType_Knight:
                        return 3;
                    case ChessPieceType.ChessPieceType_Bishop:
                        return 3.5;
                    case ChessPieceType.ChessPieceType_Queen:
                        return 10;
                    case ChessPieceType.ChessPieceType_King:
                        return 4;
                    default:
                        return -1;
                }
            }
        }
        public ChessPieceType PieceType { get; }
        public ChessColor Color { get; }
        public ChessBoardCell BoardPosition { get; set; }
        public ChessBoardCell InitialPosition { get; set; }

        private ChessBoard _board;
        public ChessBoard Board {
            get {
                return _board;
            }

            set {
                _board = value;
                _board.OnNewPieceCaptured += Board_OnNewPieceCaptured;
                _board.OnNewPieceInserted += Board_OnNewPieceInserted;
            }
        }



        public List<ChessBoardCell> posibleMovements { get; set; }
        public System.Collections.Generic.LinkedList<string> Moves { get; }
        public ChessPlayer Player { get; set; }

        public ChessPiece(ChessPieceType type, ChessPlayer player)
        {
            PieceType = type;
            Color = player.pieceColor;
            Player = player;
            Moves = new LinkedList<string>();
            PieceId = Guid.NewGuid();
        }
        public override string ToString()
        {                        
            return string.Format("{0}", ChessPGN.GetPGNNameForPiece(PieceType, false));
        }
        public void CalculatePossibleMoves()
        {
            posibleMovements = new List<ChessBoardCell>();
            switch (PieceType)
            {
                case ChessPieceType.ChessPieceType_Pawn:
                    CalcularMovimientosComoPeon();
                    break;
                case ChessPieceType.ChessPieceType_Rook:
                    break;
                case ChessPieceType.ChessPieceType_Knight:
                    break;
                case ChessPieceType.ChessPieceType_Bishop:
                    break;
                case ChessPieceType.ChessPieceType_Queen:
                    break;
                case ChessPieceType.ChessPieceType_King:
                    break;
                default:
                    break;
            }
        }
        private void CalcularMovimientosComoPeon()
        {
            ChessCellOffset[] moves = ChessCellOffset.GetPawnMoveSet(Color);

            for (int i = 0; i < moves.Count(); i++)
            {
                ChessBoardCell nextCell = Board.GetRelativeCell(BoardPosition, moves[i]);
                if (nextCell != null)
                {
                    if (i < 2) //2 first movements cannot capture pieces
                    {
                        if (nextCell.PieceInCell == null)
                        {
                            posibleMovements.Add(nextCell);
                        }
                    }
                    else {
                        if (nextCell.PieceInCell != null && nextCell.PieceInCell.Color != Color)
                        {
                            posibleMovements.Add(nextCell);
                        }
                    }
                }
            }
        }
        public Guid PieceId { get; private set; }
        public override bool Equals(object obj)
        {
            ChessPiece other = (ChessPiece)obj;
            return this.PieceId.Equals(other.PieceId);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region BOARD EVENTS
        private void Board_OnNewPieceCaptured(object sender, PieceCapturedEventArgs e)
        {
            if (e.PieceCaptured!= null && e.PieceCaptured.Equals(this))
            {
                this.BoardPosition = null;
            }
        }

        private void Board_OnNewPieceInserted(object sender, PieceInsertedEventArgs e)
        {
            if(e.PieceInserted.Equals(this))
            {
                BoardPosition = e.DestinationCell;
                InitialPosition = e.DestinationCell;
            }

        }

        public bool Equals(ChessPiece x, ChessPiece y)
        {
            return x.PieceId.Equals(y.PieceId);
        }

        public int GetHashCode(ChessPiece obj)
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
