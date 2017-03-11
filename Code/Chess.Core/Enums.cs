using System;

public enum ChessPieceType
{
    ChessPieceType_Unknown = 0,
    ChessPieceType_Pawn = 1,
    ChessPieceType_Rook = 2,
    ChessPieceType_Knight = 3,
    ChessPieceType_Bishop = 4,
    ChessPieceType_Queen = 5,
    ChessPieceType_King = 6
}

public enum ChessColor
{
    ChessColor_Unknown = 0,
    ChessColor_White = 1,
    ChessColor_Black = 2
}


public enum CellOffsetDirectionEnum
{
    CellOffsetDirectionEnum_Forward = 1,
    CellOffsetDirectionEnum_DiagonalForwardRight = 2,
    CellOffsetDirectionEnum_Right = 3,
    CellOffsetDirectionEnum_DiagonalBackwardRight = 4,
    CellOffsetDirectionEnum_Back = 5,
    CellOffsetDirectionEnum_DiagonalBackwardLeft = 6,
    CellOffsetDirectionEnum_Left = 7,
    CellOffsetDirectionEnum_DiagonalForwardLeft = 8

}

public enum ChessGameStatus
{
    ChessGameStatus_Unknown = 0,
    ChessGameStatus_NonStarted = 1,
    ChessGameStatus_Playing = 2,
    ChessGameStatus_Paused = 3,
    ChessGameStatus_Finihed = 4
}
public enum ExceptionCode
{
    Unknown = 0,
    AnotherGameInProgress,
    InvalidPGNNotation,
    AttemptToMoveOtherPlayerPiece,
    InvalidMovement,
    InvalidInitialPiecePosition,
    InvalidCellIndexes,
    InvalidAttemptToCapturePiece,
}

public enum CommandCode
{
    UNKNOWN = 0,
    BYE = 1,
    EXIT = 1,
    CREATE = 2,
    HELP = 3,
    CLEAR = 4,
    STATUS = 5,
    START = 6,
    FINISH = 7,
    TIME = 8,
    PROMPT = 9,
    MOVE = 10,
    DEBUG = 11,
}

public enum ClockState
{
    Idle = 0,
    Initialized = 1,
    Running = 2,
    Stopped = 3,
    Finished = 4
}

public enum MoveType
{
    Unknown = 0,
    StandardMove = 1,
    QueenSideCasteling = 2,
    KingSideCasteling = 3
}

[FlagsAttribute]
public enum PGNMarkersFlags
{
    NoMarkers = 0,
    Check = 1,
    Mate = 1 << 1,
    GreatMove = 1 << 2,
    Capture = 1 << 3,
    Promote = 1 << 4,

}