using System.Runtime.Serialization;

namespace AccountApi.Exceptions
{
    [Serializable]
    public class ExceptionBadRequest : Exception
    {

        public ExceptionBadRequest(string? message) : base(message)
        {
        }
    }
}