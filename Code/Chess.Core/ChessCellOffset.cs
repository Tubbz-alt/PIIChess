using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAsync.Chess
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
    }
}
