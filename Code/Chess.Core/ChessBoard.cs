using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    
    public class ChessBoard
    {
        //boardCells[ROW, COLUMN]
        public event EventHandler<PieceInsertedEventArgs> OnNewPieceInserted;
        public event EventHandler<PieceCapturedEventArgs> OnNewPieceCaptured;

        public List<ChessPiece> CapturedPieces { get; set; }        
        public ChessBoardCell[,] boardCells { get; }
        public ChessBoard()
        {
            boardCells = new ChessBoardCell[8, 8];

            Parallel.For(0, 8, (row) =>
            {
                for (byte column = 0; column < 8; column++)
                {
                    ChessBoardCell cell = new ChessBoardCell();
                    cell.Board = this;
                    cell.RowIndex = (byte)(row);
                    cell.ColumnIndex = (byte)(column);
                    boardCells[row, column] = cell;
                }
            });


            CapturedPieces = new List<ChessPiece>();
            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 7; row >= 0; row--)
            {
                for (byte column = 0; column < 8; column++)
                {
                    sb.AppendLine(boardCells[row, column].ToString());
                }
            }
            return sb.ToString();
        }        

        public ChessPiece InsertPieceInBoard(ChessPiece piece, string algebraicPosition)
        {
            ChessBoardCell cell = this[algebraicPosition];
            if (cell != null)
            {
                //Add the piece to the board
                cell.PieceInCell = piece;
                piece.Board = this; 
                //Piece Expected Action: Update its internal fields.
                //                       Initial Position, CurrentPosition
                OnNewPieceInserted?.Invoke(this, new PieceInsertedEventArgs(piece, cell));
                return piece;
            }
            else {
                throw new ChessExceptionInvalidGameAction(ExceptionCode.InvalidInitialPiecePosition, algebraicPosition);
            }

        }

        public void MovePiece(ChessPiece piece, string algebraicPosition)
        {
            byte rowIndex = ChessPGN.UNKNOWN_INDEX;
            byte colIndex = ChessPGN.UNKNOWN_INDEX;
            ChessPGN.AlgebraicPositionToIndexes(algebraicPosition, ref rowIndex, ref colIndex);
            MovePiece(piece, rowIndex, colIndex);
        }
        public void MovePiece(ChessPiece piece, byte rowIndex, byte colIndex)
        {
            try
            {
                //remove the piece from its original position
                ChessBoardCell originalPosition = piece.BoardPosition;
                originalPosition.PieceInCell = null;

                //Obtain the new position
                ChessBoardCell cell = this[rowIndex, colIndex];
                if (cell != null)
                {
                    //Check if the new position capture a piece.
                    if (cell.PieceInCell != null)
                    {
                        if (cell.PieceInCell.Color != piece.Color)
                        {
                            //Capture the other piece 
                            ChessPiece capturedPiece = cell.PieceInCell;
                            CapturedPieces.Add(capturedPiece);
                            
                            //Player Expected Action: The player remove the piece of his active piece list.
                            //Piece Expected Action: Update its internal position record.
                            OnNewPieceCaptured?.Invoke(this, new PieceCapturedEventArgs(capturedPiece));

                            //Remove the captured piece from board.
                            cell.PieceInCell = null;
                        }
                        else {
                            throw new ChessExceptionInvalidGameAction(ExceptionCode.InvalidAttemptToCapturePiece, "The piece that you are trying to capture has the same color than the moved piece");
                        }

                    }
                    
                    cell.PieceInCell = piece;
                    piece.BoardPosition = cell;
                }
            }
            catch (Exception)
            {
                throw new ChessExceptionInvalidGameAction(ExceptionCode.InvalidCellIndexes, "Row Index: " + rowIndex + ", Col Index: " + colIndex);
            }

            
        }


        /// <summary>
        /// When we Refers to cells by an algebraic position, the indexes are 1-based
        /// </summary>
        /// <param name="algebraicPosition"></param>
        /// <returns></returns>
        public ChessBoardCell this[string algebraicPosition]
        {
            get
            {
                byte row = 0;
                byte column = 0;
                ChessPGN.AlgebraicPositionToIndexes(algebraicPosition, ref row, ref column);
                if(row != ChessPGN.UNKNOWN_INDEX && column != ChessPGN.UNKNOWN_INDEX)
                { 
                    return boardCells[row, column];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// When we refers to cell by column and row, the indexes are 0-based but we shouls keep the ChessIndex Mode
        /// in which Row = 1 ==> index 7
        /// </summary>
        /// <param name="columnIndex">The cell's base 0 index for the column</param>
        /// <param name="rowIndex">The cell's base 0 index for the row</param>
        /// <returns></returns>
        public ChessBoardCell this[int rowIndex, int columnIndex]
        {
            get
            {
                if (columnIndex >= 0 && rowIndex >= 0 &&
                    rowIndex < boardCells.GetLongLength(0) && columnIndex < boardCells.GetLongLength(1))
                {
                    return boardCells[rowIndex, columnIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        public ChessBoardCell GetRelativeCell(ChessBoardCell baseCell, ChessCellOffset offset)
        {
            int newRowIndex = baseCell.RowIndex + offset.VerticalOffset;
            int newColumnIndex = baseCell.ColumnIndex + offset.HorizontalOffset;
            return this[newRowIndex, newColumnIndex];
        }

        public ChessBoardCell[] GetRow(byte rowIndex)
        {
            return new ChessBoardCell[] {
                boardCells[rowIndex,0],
                boardCells[rowIndex,1],
                boardCells[rowIndex,2],
                boardCells[rowIndex,3],
                boardCells[rowIndex,4],
                boardCells[rowIndex,5],
                boardCells[rowIndex,6],
                boardCells[rowIndex,7]
            };
        }

        public ChessBoardCell[] GetCol(byte colIndex)
        {
            return new ChessBoardCell[] {
                boardCells[0,colIndex],
                boardCells[1,colIndex],
                boardCells[2,colIndex],
                boardCells[3,colIndex],
                boardCells[4,colIndex],
                boardCells[5,colIndex],
                boardCells[6,colIndex],
                boardCells[7,colIndex]
            };
        }

    }

    public class PieceInsertedEventArgs:EventArgs
    {
        public PieceInsertedEventArgs(ChessPiece piece, ChessBoardCell destinationCell)
        {
            this.PieceInserted = piece;
            this.DestinationCell = destinationCell;
        }
        public ChessPiece PieceInserted { get; set; }
        public ChessBoardCell DestinationCell { get; set; }
    }

    public class PieceCapturedEventArgs : EventArgs
    {
        public PieceCapturedEventArgs(ChessPiece piece)
        {
            this.PieceCaptured = piece;
        }
        public ChessPiece PieceCaptured { get; set; }
    }

}
