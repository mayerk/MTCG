using System.Runtime.Serialization;

namespace MTCG.BLL.Exceptions
{
    [Serializable]
    internal class DuplicateCardException : Exception
    {
        public DuplicateCardException()
        {
        }

        public DuplicateCardException(string? message) : base(message)
        {
        }

        public DuplicateCardException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateCardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}