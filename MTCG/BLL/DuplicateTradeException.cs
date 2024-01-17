using System.Runtime.Serialization;

namespace MTCG.BLL {
    [Serializable]
    internal class DuplicateTradeException : Exception {
        public DuplicateTradeException() {
        }

        public DuplicateTradeException(string? message) : base(message) {
        }

        public DuplicateTradeException(string? message, Exception? innerException) : base(message, innerException) {
        }

        protected DuplicateTradeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}