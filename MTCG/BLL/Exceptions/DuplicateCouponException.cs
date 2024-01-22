using System.Runtime.Serialization;

namespace MTCG.BLL.Exceptions
{
    [Serializable]
    public class DuplicateCouponException : Exception
    {
        public DuplicateCouponException()
        {
        }

        public DuplicateCouponException(string? message) : base(message)
        {
        }

        public DuplicateCouponException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateCouponException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}