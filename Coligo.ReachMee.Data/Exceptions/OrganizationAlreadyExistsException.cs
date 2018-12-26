using System;
using System.Runtime.Serialization;

namespace Coligo.ReachMee.Data.Exceptions
{
    [Serializable]
    public class OrganizationAlreadyExistsException : Exception
    {
        public OrganizationAlreadyExistsException()
        {
        }

        public OrganizationAlreadyExistsException(string message) : base(message)
        {
        }

        public OrganizationAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrganizationAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}