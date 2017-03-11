using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAsync
{
    public class ChessException:Exception
    {
        public ChessException() : base("Unknown Exception")
        {
            ExceptionCode = ExceptionCode.Unknown;
        }
        public ChessException(string message) : base(message){ }
        public ChessException(ExceptionCode exceptionCode, string message) : base(message) {
            exceptionCode = ExceptionCode;
        }
        public ExceptionCode ExceptionCode { get; set; } 
    }

    public class ChessExceptionInvalidGameAction: ChessException
    {
        public ChessExceptionInvalidGameAction(ExceptionCode exceptionCode, string message) : base(exceptionCode, message) { }
      
    }

    public class ChessExceptionInvalidPGNNotation : ChessException
    {
        public ChessExceptionInvalidPGNNotation() : base(ExceptionCode.InvalidPGNNotation, "Unknown Exception")
        {
        }
        public ChessExceptionInvalidPGNNotation(string message) : base(ExceptionCode.InvalidPGNNotation, message)
        {
        }
        public ChessExceptionInvalidPGNNotation(ExceptionCode exceptionCode, string message) : base(exceptionCode, message) { }
    }
}
