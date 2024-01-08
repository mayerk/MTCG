﻿using System.Runtime.Serialization;

namespace MTCG.BLL {
    [Serializable]
    internal class NoPackageAvailableException : Exception {
        public NoPackageAvailableException() {
        }

        public NoPackageAvailableException(string? message) : base(message) {
        }

        public NoPackageAvailableException(string? message, Exception? innerException) : base(message, innerException) {
        }

        protected NoPackageAvailableException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}