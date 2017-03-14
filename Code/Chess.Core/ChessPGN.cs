using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public static class ChessPGN
    {        
        public const byte UNKNOWN_INDEX = 99;
        public const char UNKNOWN_COL_NAME = '_';

        public const char PGN_PIECE_PAWN = '\0';
        public const char PGN_PIECE_PAWN_EXPLICIT = 'P';
        public const char PGN_PIECE_KING = 'K';
        public const char PGN_PIECE_QUEEN = 'Q';
        public const char PGN_PIECE_BISHOP = 'B';
        public const char PGN_PIECE_KNIGHT = 'N';
        public const char PGN_PIECE_ROOK = 'R';
        public const char PGN_PIECE_UNKNOWN = '\n';
        

        public const string PGN_NOTATION_MOVE_KINKGSIDE_CASTLING = "0-0";
        public const string PGN_NOTATION_MOVE_QUEENGSIDE_CASTLING = "0-0-0";

        public const char PGN_MARKER_CHECK = '+';
        public const char PGN_MARKER_CHECKMATE = '#';
        public const char PGN_MARKER_PROMOTION = '=';
        public const char PGN_MARKER_CAPTURE = 'x';


        public static readonly char[] PGN_PIECES_NAMES;
        public static readonly string[] PGN_SPECIAL_MOVES;
        public static readonly char[] PGN_MARKERS;
        public static readonly char[] PGN_COLUMN_NAMES;
        public static readonly char[] PGN_ROW_NAMES;
        static ChessPGN()
        {
            PGN_PIECES_NAMES = new char[] { PGN_PIECE_PAWN_EXPLICIT,
                                            PGN_PIECE_KING,
                                            PGN_PIECE_QUEEN,
                                            PGN_PIECE_BISHOP,
                                            PGN_PIECE_KNIGHT,
                                            PGN_PIECE_ROOK
            };

            PGN_SPECIAL_MOVES = new string[]{
                                    PGN_NOTATION_MOVE_KINKGSIDE_CASTLING,
                                    PGN_NOTATION_MOVE_QUEENGSIDE_CASTLING
            };

            PGN_MARKERS = new char[] {
                PGN_MARKER_CHECK,
                PGN_MARKER_CHECKMATE,
                PGN_MARKER_PROMOTION,
                PGN_MARKER_CAPTURE

            };

            PGN_COLUMN_NAMES = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'};
            PGN_ROW_NAMES = new char[] { '1', '2', '3', '4', '5', '6', '7', '8'};

        }

        public static char PGNColumnNameFromColumnNumber(byte columnNumber)
        {
            return (char)(columnNumber + PGN_COLUMN_NAMES[0]);
        }

        public static byte ColumnIndexFromPGNColumnName(char columnName)
        {
            return (byte) (columnName - PGN_COLUMN_NAMES[0]);
        }

        public static void AlgebraicPositionToIndexes(string algebraicPosition, ref byte rowIndex, ref byte colIndex)
        {
            if (algebraicPosition.Length == 2)
            {
                algebraicPosition = algebraicPosition.ToLower();
                char columnAsChar = algebraicPosition[0];
                char rowAsChar = algebraicPosition[1];

                rowIndex = (byte)Char.GetNumericValue((char)(rowAsChar - 1));
                colIndex = ColumnIndexFromPGNColumnName(columnAsChar);
            }
            else
            {
                rowIndex = UNKNOWN_INDEX;
                colIndex = UNKNOWN_INDEX;
            }
        }

        public static bool IsValidAlgebraicPosition(string algebraicPosition)
        {
                byte rowIndex = UNKNOWN_INDEX;
                byte colIndex = UNKNOWN_INDEX;
                AlgebraicPositionToIndexes(algebraicPosition, ref rowIndex, ref colIndex);

                return (rowIndex >= 0 && rowIndex <= 7 && colIndex >= 0 && colIndex <= 7);
        }
        public static char GetPGNNameForPiece(ChessPieceType pieceType, bool useEmptyForPawn)
        {
            switch (pieceType)
            {
                case ChessPieceType.ChessPieceType_Pawn:
                    if (useEmptyForPawn)
                    {
                        return PGN_PIECE_PAWN;
                    }
                    else
                    {
                        return PGN_PIECE_PAWN_EXPLICIT;
                    }                    
                case ChessPieceType.ChessPieceType_Rook:
                    return PGN_PIECE_ROOK;
                case ChessPieceType.ChessPieceType_Knight:
                    return PGN_PIECE_KNIGHT; 
                case ChessPieceType.ChessPieceType_Bishop:
                    return PGN_PIECE_BISHOP;
                case ChessPieceType.ChessPieceType_Queen:
                    return PGN_PIECE_QUEEN;
                case ChessPieceType.ChessPieceType_King:
                    return PGN_PIECE_KING;
                default:
                    return PGN_PIECE_UNKNOWN;
            }
        }

        public static ChessPieceType GetPieceTypeFromPGNName(char PGNPieceName)
        {
            switch (PGNPieceName)
            {
                case PGN_PIECE_PAWN:
                case PGN_PIECE_PAWN_EXPLICIT:
                    return ChessPieceType.ChessPieceType_Pawn;
                case PGN_PIECE_BISHOP:
                    return ChessPieceType.ChessPieceType_Bishop;
                case PGN_PIECE_KNIGHT:
                    return ChessPieceType.ChessPieceType_Knight;
                case PGN_PIECE_ROOK:
                    return ChessPieceType.ChessPieceType_Rook;
                case PGN_PIECE_QUEEN:
                    return ChessPieceType.ChessPieceType_Queen;
                case PGN_PIECE_KING:
                    return ChessPieceType.ChessPieceType_King;
                default:
                    return ChessPieceType.ChessPieceType_Unknown;
            }
        }

        public static ChessMove GetMoveFromPGN(string PGNMove, ChessPlayer player,ChessBoard board)
        {
            #region INITIALIZATION
            string OriginalPGNMove = PGNMove;
            //string destinationAddress = "";
            bool isCheck = false;
            bool isMate = false;
            bool isCapture = false;
            bool isPromotion = false;
            byte destinationRow = UNKNOWN_INDEX;
            char destinationCol = UNKNOWN_COL_NAME;
            byte originRow = UNKNOWN_INDEX;
            char originCol = UNKNOWN_COL_NAME;
            ChessBoardCell fromCell = null;
            ChessBoardCell toCell = null;

            ChessPieceType pieceToMove = ChessPieceType.ChessPieceType_Unknown;
            ChessPieceType pieceToPromote = ChessPieceType.ChessPieceType_Unknown;

            // EXAMPLES, POSSIBLE CODES
            //      '0-0'   Kingside Casteling
            //      '0-0-0' Queen Side Casteling
            //      'h4'    Pawn to h4                   <-- (implicit piece name for pawn) 
            //      'Kh4'   King to h4
            //      'Bah4'  Bishop in column a to h4     <-- Column Name provided Desambiguation Level 1
            //      'N3h4'  Knight in Row 3 to h4        <-- Row Number provided Desambiguation Level 2
            //      'Nb1c3' Knight in b1 to c3           <-- Column Name and Row Number provided Desambiguation Level 3
            //      'dd4'   Pawn in Column d to d4       <-- Pawn (Implicit Reference) Desambiguation Level 1
            //      '3d4'   Pawn in Row 3 to d4 (??)     <-- Pawn (Implicit Reference) Desambiguation Level 2
            //      'd3d4'  Pawn in d3 to d4 (??)        <-- Pawn (Implicit Reference) Desambiguation Level 3
            //      '[PGN]=Q'    Pawn Promoted to Quen      
            //      'x[PGN]=Q'   Pawn Capture and Promoted to Quen     
            //       []x[]       Movement With Capture
            //       '[]+'       Movement and Check
            //       '[]#'       Movement and Check Mate
            //      'x[PGN]=Q#'   Pawn Capture and Promoted to Quen and Check    

            if (PGNMove.Length < 2)
            {
                throw new ChessExceptionInvalidPGNNotation("Invalid Notation: " + OriginalPGNMove);
            }
            #endregion
            #region SPECIAL MOVES
            if (PGN_SPECIAL_MOVES.Contains(PGNMove))
            {
                if (PGNMove.Equals(PGN_NOTATION_MOVE_KINKGSIDE_CASTLING))
                {
                    return ChessMove.CreateKingSideCastlingMove();
                }
                if (PGNMove.Equals(PGN_NOTATION_MOVE_QUEENGSIDE_CASTLING))
                {
                    return ChessMove.CreateQueenSideCastlingMove();
                }
                throw new ChessExceptionInvalidPGNNotation("Unkwnon Special Notation [" + OriginalPGNMove + "]");
            }
            #endregion
            #region SPECIAL MARKERS

            if (PGNMove.IndexOfAny(PGN_MARKERS) != -1)
            {
                foreach (char marker in PGN_MARKERS)
                {
                    switch (marker)
                    {
                        case PGN_MARKER_CAPTURE:
                            isCapture = true;
                            break;
                        case PGN_MARKER_CHECK:
                            isCheck = true;
                            break;
                        case PGN_MARKER_CHECKMATE:
                            isMate = true;
                            break;
                        case PGN_MARKER_PROMOTION:
                            isPromotion = true;                            
                            break;
                    }
                } 

                PGNMove = PGNMove.Replace(new string(PGN_MARKER_CAPTURE,1), "")
                                          .Replace(new string(PGN_MARKER_CHECK,1), "")
                                          .Replace(new string(PGN_MARKER_CHECKMATE, 1), "")
                                          .Replace(new string(PGN_MARKER_PROMOTION, 1), "");

            }

            #endregion
            #region PROCESS PROMOTION
            if (isPromotion)
            {
                char promotedPieceCode = PGNMove[PGNMove.Length - 1];
                pieceToPromote = GetPieceTypeFromPGNName(promotedPieceCode);
                PGNMove = PGNMove.Substring(0, PGNMove.Length - 1);
            }
            #endregion
            #region REMOVE IMPLICIT PIECE NAME

            if (PGNMove.IndexOfAny(PGN_PIECES_NAMES) == -1)
            {
                //IMPLICIT PAWN
                //       EXAMPLE 
                //      'h4' pawn to h4
                PGNMove = PGN_PIECE_PAWN_EXPLICIT + PGNMove;
                //NO MORE IMPLICIT REFERENCES!!!!
            }
            #endregion
            #region OBTAIN DESTINY BOARD CELL
            {
                char row = PGNMove[PGNMove.Length - 1];
                char col = PGNMove[PGNMove.Length - 2];
                if (!(PGN_COLUMN_NAMES.Contains(col) && PGN_ROW_NAMES.Contains(row)))
                {
                    throw new ChessExceptionInvalidPGNNotation("Invalid Notation: " + OriginalPGNMove);
                }
                else
                {
                    destinationRow = byte.Parse(new string(row, 1));
                    destinationCol = col;
                }
                string algebraicCoordinates = string.Format("{0}{1}", col, row);
                toCell = board[algebraicCoordinates];
                PGNMove = PGNMove.Substring(0, PGNMove.Length - 2);
            }

            #endregion
            #region OBTAIN PIECE TYPE TO MOVE
            {
                char firstChar = PGNMove[0];
                if (PGN_PIECES_NAMES.Contains(firstChar))
                {
                    pieceToMove = GetPieceTypeFromPGNName(firstChar);
                    PGNMove = PGNMove.Substring(1, PGNMove.Length - 1);
                }
                else {
                    throw new ChessExceptionInvalidPGNNotation("Invalid Notation: " + OriginalPGNMove);
                }
            }
            #endregion
            //At this point:
            //   - Special Markers Processed and Codes were Removed 
            //   - Promotion Solved and Codes Removed
            //   - Piece To Move Identified and Code was Removed
            //   - PNG String has as least 1 positions.

            #region DESAMBIGUATION ANALISYS
            if (PGNMove.Length > 0)
            {
                //Desambiguation Required
                if (PGNMove.Length == 1)
                {
                    //Desambiguation Level 1 or 2 Required
                    //Disambiguation Level 1 or 2 Required
                    if (PGN_COLUMN_NAMES.Contains(PGNMove[0]))
                    {
                        // Disambiguation LEVEL 1
                        originCol = PGNMove[0];
                    }
                    else if (PGN_ROW_NAMES.Contains(PGNMove[0]))
                    {
                        // Disambiguation LEVEL 2                        
                        originRow = (byte)char.GetNumericValue(PGNMove[0]);
                    }
                    else
                    {
                        throw new ChessExceptionInvalidPGNNotation("Invalid Notation: " + OriginalPGNMove);
                    }
                }
                else if (PGNMove.Length == 2)
                {
                    //Desambiguation Level 3 Required
                    if (PGN_COLUMN_NAMES.Contains(PGNMove[0]) && PGN_ROW_NAMES.Contains(PGNMove[1]))
                    {
                        // Disambiguation LEVEL 3
                        originCol = PGNMove[0];
                        originRow = (byte)char.GetNumericValue(PGNMove[1]);
                    }
                    else
                    {
                        throw new ChessExceptionInvalidPGNNotation("Invalid Notation: " + OriginalPGNMove);
                    }
                }
                else {
                    throw new ChessExceptionInvalidPGNNotation("Invalid Notation: " + OriginalPGNMove);
                }
            }


            #endregion

            #region OBTAIN ORIGIN BOARD CELL
            if (originCol != UNKNOWN_COL_NAME && originRow != UNKNOWN_INDEX)
            {
                string algebraicCoordinates = string.Format("{0}{1}", originCol, originRow);
                fromCell = board[algebraicCoordinates];
            }else {
                ChessBoardCell[] cells = null;
                if (originCol != UNKNOWN_COL_NAME)
                {
                    byte colIndex = ColumnIndexFromPGNColumnName(originCol);
                    cells = board.GetCol(colIndex);
                }
                else if (originRow != UNKNOWN_INDEX)
                {
                    cells = board.GetRow(originRow);
                } else {
                    //Row and Column are implicit I need to search piece of the provided type that can move to 
                    //the ToCell
                    throw new NotImplementedException();
                }

                if (cells != null)
                {
                    var query = from cell in cells
                                where cell.PieceInCell.Color == player.pieceColor && cell.PieceInCell.PieceType == pieceToMove
                                select cell;
                    ChessBoardCell[] ResultCell= query.ToArray();
                    if (ResultCell.Count() == 1)
                    {
                        fromCell = ResultCell[0];
                    } else {
                        throw new ChessExceptionInvalidPGNNotation("PGN Notation cannot determine the piece to move: " + OriginalPGNMove);
                    }

                }else{

                    
                    throw new ChessExceptionInvalidPGNNotation("Invalid Notation: " + OriginalPGNMove);
                }
            }
            #endregion

            #region PGN FLAGS
            PGNMarkersFlags flags = PGNMarkersFlags.NoMarkers;
            if (isCheck)
            {
                flags = flags | PGNMarkersFlags.Check;
            }
            if (isMate)
            {
                flags = flags | PGNMarkersFlags.Mate;
            }
            if (isPromotion)
            {
                flags = flags | PGNMarkersFlags.Promote;
            }
            if (isCapture)
            {
                flags = flags | PGNMarkersFlags.Capture;
            }
            #endregion

            return ChessMove.CreateStandardChessMove(fromCell.PieceInCell, toCell, pieceToPromote, flags);         
        }
    }
}
