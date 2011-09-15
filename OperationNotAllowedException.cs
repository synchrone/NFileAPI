using System;
using System.Windows.Browser;

namespace NFileAPI
{
    [ScriptableType]
    public class OperationNotAllowedException : Exception
    {
        public const short NOT_ALLOWED_ERR = 1;

        public short code { get; internal set; }

        internal OperationNotAllowedException(short code) {
            this.code = code;
        }
        internal OperationNotAllowedException() : this(NOT_ALLOWED_ERR) {}
    }
}
