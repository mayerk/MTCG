using System.Runtime.Serialization;

namespace MTCG.DAL {
    [Serializable]
    internal class DuplicatePackageException : Exception {
        public DuplicatePackageException() {
        }

        public DuplicatePackageException(string? message) : base(message) {
        }

        public DuplicatePackageException(string? message, Exception? innerException) : base(message, innerException) {
        }

        protected DuplicatePackageException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}