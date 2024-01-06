﻿using System.Runtime.Serialization;

namespace MTCG.HttpServer
{
    [Serializable]
    internal class RouteNotAuthenticatedException : Exception
    {
        public RouteNotAuthenticatedException()
        {
        }

        public RouteNotAuthenticatedException(string? message) : base(message)
        {
        }

        public RouteNotAuthenticatedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RouteNotAuthenticatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}