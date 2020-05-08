using System;

namespace Sindikat.Identity.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {
        }
        public ValidationException(object validationErrors) : base()
        {
            ValidationErrors = validationErrors;
        }

        public object ValidationErrors { get; private set; }
    }
}
