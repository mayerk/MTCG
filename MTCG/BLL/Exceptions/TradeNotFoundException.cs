using System.Runtime.Serialization;

namespace MTCG.BLL.Exceptions
{
    [Serializable]
    internal class TradeNotFoundException : Exception
    {
        public TradeNotFoundException()
        {
        }

        public TradeNotFoundException(string? message) : base(message)
        {
        }

        public TradeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TradeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}