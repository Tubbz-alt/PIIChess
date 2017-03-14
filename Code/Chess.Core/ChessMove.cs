using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class ChessMove
    {
        #region Static Factory

        public static ChessMove CreateStandardChessMove(ChessBoardCell fromCell, ChessBoardCell toCell)
        {
            return new ChessMove(MoveType.StandardMove, fromCell.PieceInCell, toCell, ChessPieceType.ChessPieceType_Unknown, PGNMarkersFlags.NoMarkers);
        }

        public static ChessMove CreateStandardChessMove(string PGNNotationMove, ChessPlayer player, ChessBoard board)
        {
            return ChessPGN.GetMoveFromPGN(PGNNotationMove, player, board);
        }
        public static ChessMove CreateStandardChessMove(ChessPiece piece, ChessBoardCell toCell)
        {
            return new ChessMove(MoveType.StandardMove, piece, toCell, ChessPieceType.ChessPieceType_Unknown, PGNMarkersFlags.NoMarkers);
        }
        public static ChessMove CreateStandardChessMove(ChessPiece piece, ChessBoardCell toCell, ChessPieceType promoteTo)
        {
            return new ChessMove(MoveType.StandardMove, piece, toCell, ChessPieceType.ChessPieceType_Unknown, PGNMarkersFlags.NoMarkers);
        }
        public static ChessMove CreateStandardChessMove(ChessPiece piece, ChessBoardCell toCell, ChessPieceType promoteTo, PGNMarkersFlags pgnMarkers)
        {
            return new ChessMove(MoveType.StandardMove, piece, toCell, ChessPieceType.ChessPieceType_Unknown, pgnMarkers);
        }
        public static ChessMove CreateKingSideCastlingMove()
        {
            return new ChessMove(MoveType.KingSideCasteling, null, null, ChessPieceType.ChessPieceType_Unknown, PGNMarkersFlags.NoMarkers);
        }
        public static ChessMove CreateKingSideCastlingMove(PGNMarkersFlags pgnMarkers)
        {
            return new ChessMove(MoveType.KingSideCasteling, null, null, ChessPieceType.ChessPieceType_Unknown, pgnMarkers);
        }
        public static ChessMove CreateQueenSideCastlingMove()
        {
            return new ChessMove(MoveType.QueenSideCasteling, null, null, ChessPieceType.ChessPieceType_Unknown, PGNMarkersFlags.NoMarkers);
        }
        public static ChessMove CreateQueenSideCastlingMove(PGNMarkersFlags pgnMarkers)
        {
            return new ChessMove(MoveType.QueenSideCasteling, null, null, ChessPieceType.ChessPieceType_Unknown, pgnMarkers);
        }



        #endregion

        private ChessMove(MoveType type, ChessPiece piece, ChessBoardCell toCell, ChessPieceType promoteTo, PGNMarkersFlags pgnMarkers)
        {
            Type = type;
            PGNMarkers = pgnMarkers;
            if (Type == MoveType.StandardMove)
            {
                Origin = piece.BoardPosition;
                Destiny = toCell;
                PromoteTo = promoteTo;
            }
            else {
                Origin = null;
                Destiny = null;
            }
            
        }

        public MoveType Type { get; set; }
        public ChessBoardCell Origin { get; private set; }               
        public ChessBoardCell Destiny { get; private set; }
        public ChessPieceType PromoteTo { get; set; }
        public bool MoveCapturePiece { get {
                return Origin.PieceInCell != null && Destiny.PieceInCell != null &&
                    Origin.PieceInCell.Color != Destiny.PieceInCell.Color;
            } }
        public ChessPiece PieceToMove {
            get {
                return Origin.PieceInCell;
            }
        }    
        public int MoveRank { get; set; }
        public bool MoveAccepted { get; set; }
        public PGNMarkersFlags PGNMarkers;

        public override string ToString()
        {
            switch (Type)
            {
                case MoveType.StandardMove:
                    if (Origin != null && Origin.PieceInCell != null && Destiny != null)
                    {
                        return Origin.PieceInCell.ToString() + " " + Origin.ToString() + Destiny.ToString();
                    }
                    else {
                        return "Invalid Move";
                    }                    
                case MoveType.QueenSideCasteling:
                    return ChessPGN.PGN_NOTATION_MOVE_QUEENGSIDE_CASTLING;
                case MoveType.KingSideCasteling:
                    return ChessPGN.PGN_NOTATION_MOVE_KINKGSIDE_CASTLING;
                default:
                    return "Invalid Move";
            }
            
        }
    }
}
