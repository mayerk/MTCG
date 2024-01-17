using System.Runtime.Serialization;

namespace MTCG.BLL {
    [Serializable]
    internal class DeckNotFoundException : Exception {
        public DeckNotFoundException() {
        }

        public DeckNotFoundException(string? message) : base(message) {
        }

        public DeckNotFoundException(string? message, Exception? innerException) : base(message, innerException) {
        }

        protected DeckNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}