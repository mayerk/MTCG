using System.Runtime.Serialization;

namespace MTCG.BLL.Exceptions
{
    [Serializable]
    internal class CouponNotFoundException : Exception
    {
        public CouponNotFoundException()
        {
        }

        public CouponNotFoundException(string? message) : base(message)
        {
        }

        public CouponNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CouponNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}