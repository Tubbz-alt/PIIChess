using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public struct ChessCellOffset
    {

        public int HorizontalOffset;
        public int VerticalOffset;
        public ChessCellOffset(int verticalOffset, int horizontalOffset)
        {
            this.HorizontalOffset = horizontalOffset;
            this.VerticalOffset = verticalOffset;
        }
        public ChessCellOffset(CellOffsetDirectionEnum vectorDirection)
        {
            this.HorizontalOffset = 0;
            this.VerticalOffset = 0;
            InitializeCellOffset(vectorDirection, 1);
        }
        public ChessCellOffset(CellOffsetDirectionEnum vectorDirection, int vectorModule)
        {
            this.HorizontalOffset = 0;
            this.VerticalOffset = 0;
            InitializeCellOffset(vectorDirection, vectorModule);
        }
        private void InitializeCellOffset(CellOffsetDirectionEnum vectorDirection, int vectorModule)
        {
            switch (vectorDirection)
            {
                case CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward:
                    this.HorizontalOffset = 0;
                    this.VerticalOffset = vectorModule;
                    break;
                case CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight:
                    this.HorizontalOffset = vectorModule;
                    this.VerticalOffset = vectorModule;
                    break;
                case CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right:
                    this.HorizontalOffset = 0;
                    this.VerticalOffset = vectorModule;
                    break;
                case CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight:
                    this.HorizontalOffset = vectorModule;
                    this.VerticalOffset = -vectorModule;
                    break;
                case CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back:
                    this.HorizontalOffset = -vectorModule;
                    this.VerticalOffset = 0;
                    break;
                case CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft:
                    this.HorizontalOffset = -vectorModule;
                    this.VerticalOffset = -vectorModule;
                    break;
                case CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left:
                    this.HorizontalOffset = -vectorModule;
                    this.VerticalOffset = 0;
                    break;
                case CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft:
                    this.HorizontalOffset = -vectorModule;
                    this.VerticalOffset = vectorModule;
                    break;
                default:
                    this.HorizontalOffset = 0;
                    this.VerticalOffset = 0;
                    break;
            }
        }
        public static ChessCellOffset operator +(ChessCellOffset operator1, ChessCellOffset operator2)
        {
            int totalHorizontalOffset = operator1.HorizontalOffset + operator2.HorizontalOffset;
            int totalVerticalOffset = operator1.VerticalOffset + operator2.VerticalOffset;
            ChessCellOffset newOffset = new ChessCellOffset(totalVerticalOffset, totalHorizontalOffset);
            return newOffset;
        }

        public static ChessCellOffset[] GetRookMoveSet()
        {
            ChessCellOffset[] rook = new ChessCellOffset[26];
            rook[0] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left, 7);
            rook[1] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left, 6);
            rook[2] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left, 5);
            rook[3] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left, 4);
            rook[4] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left, 3);
            rook[5] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left, 2);
            rook[6] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left, 1);

            rook[7] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right, 7);
            rook[8] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right, 6);
            rook[9] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right, 5);
            rook[10] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right, 4);
            rook[11] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right, 3);
            rook[12] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right, 2);
            rook[13] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right, 1);

            rook[14] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward, 7);
            rook[15] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward, 6);
            rook[16] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward, 5);
            rook[17] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward, 4);
            rook[18] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward, 3);
            rook[19] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward, 2);
            rook[20] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward, 1);

            rook[21] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back, 7);
            rook[22] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back, 6);
            rook[23] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back, 5);
            rook[24] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back, 4);
            rook[25] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back, 3);
            rook[26] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back, 2);
            rook[27] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back, 1);

            return rook;
        }

        public static ChessCellOffset[] GetBishopMoveSet()
        {
            ChessCellOffset[] bishop = new ChessCellOffset[26];
            bishop[0] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft, 7);
            bishop[1] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft, 6);
            bishop[2] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft, 5);
            bishop[3] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft, 4);
            bishop[4] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft, 3);
            bishop[5] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft, 2);
            bishop[6] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft, 1);

            bishop[7] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight, 7);
            bishop[8] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight, 6);
            bishop[9] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight, 5);
            bishop[10] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight, 4);
            bishop[11] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight, 3);
            bishop[12] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight, 2);
            bishop[13] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight, 1);

            bishop[14] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft, 7);
            bishop[15] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft, 6);
            bishop[16] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft, 5);
            bishop[17] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft, 4);
            bishop[18] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft, 3);
            bishop[19] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft, 2);
            bishop[20] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft, 1);

            bishop[21] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight, 7);
            bishop[22] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight, 6);
            bishop[23] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight, 5);
            bishop[24] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight, 4);
            bishop[25] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight, 3);
            bishop[26] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight, 2);
            bishop[27] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight, 1);

            return bishop;
        }

        public static ChessCellOffset[] GetKingMoveSet()
        {
            ChessCellOffset[] king = new ChessCellOffset[8];
            king[0] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back);
            king[1] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft);
            king[2] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight);
            king[3] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft);
            king[4] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight);
            king[5] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward);
            king[6] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Left);
            king[7] = new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Right);
            return king;
        }

        public static ChessCellOffset[] GetQueenMoveSet() {
            ChessCellOffset[] queen = new ChessCellOffset[26*2];
            ChessCellOffset[] rook = GetRookMoveSet();
            ChessCellOffset[] bishop = GetBishopMoveSet();
            rook.CopyTo(queen, 0);
            bishop.CopyTo(queen, 26);

            return queen;
        }

        public static ChessCellOffset[] GetKnightMoveSet()
        {
            ChessCellOffset[] knight = new ChessCellOffset[8];
            knight[0] = new ChessCellOffset(2, 1);
            knight[1] = new ChessCellOffset(1, 2);

            knight[2] = new ChessCellOffset(2, -1);
            knight[3] = new ChessCellOffset(1, -2);


            knight[4] = new ChessCellOffset(-2, 1);
            knight[5] = new ChessCellOffset(-1, 2);

            knight[6] = new ChessCellOffset(-2, -1);
            knight[7] = new ChessCellOffset(-1, -2);
            return knight;

        }

        public static ChessCellOffset[] GetPawnMoveSet(ChessColor Color)
        {
            if (Color == ChessColor.ChessColor_White)
            {
                ChessCellOffset[] whiteMovements = new ChessCellOffset[] {
                                                new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward),
                                                new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Forward, 2),
                                                new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardLeft),
                                                new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalForwardRight),
                };
                return whiteMovements;
            }
            else if (Color == ChessColor.ChessColor_Black)
            {
                ChessCellOffset[] blackMovements = new ChessCellOffset[] {
                                                new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back),
                                                new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_Back, 2),
                                                new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardRight),
                                                new ChessCellOffset(CellOffsetDirectionEnum.CellOffsetDirectionEnum_DiagonalBackwardLeft),
                };
                return blackMovements;
            }
            else {
                throw new ChessException(ExceptionCode.InvalidColorPiece, "Attempt to get the move set fora pawn of a unkwnon color");
            }

        }
    }
}
