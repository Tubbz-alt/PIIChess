using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAsync.Chess
{
    public class ChessBoardCell
    {
        public byte RowIndex { get; set; }      
        public byte ColumnIndex { get; set; }
        public byte RowPGNName {
            get {
                return (byte)(RowIndex + 1);
            }
        }
        public char ColumnPGNName { get
            {
                return ChessPGN.PGNColumnNameFromColumnNumber(ColumnIndex);
            }
        }

        public ChessPiece PieceInCell { get; set; }
        public ChessColor CellColor{
            get{
                return (this.ColumnIndex % 2 == this.RowIndex % 2) ? ChessColor.ChessColor_Black : ChessColor.ChessColor_White;
            }
         }
        public ChessBoard Board { get; set; }
        public string CellAlgebraicName {
            get {
                return ColumnPGNName.ToString() + RowPGNName.ToString();
            }
        }

        public override string ToString()
        {            
            return CellAlgebraicName;
        }
        public override bool Equals(object obj)
        {
            ChessBoardCell other = (ChessBoardCell)obj;
            return this.CellAlgebraicName == other.CellAlgebraicName;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
