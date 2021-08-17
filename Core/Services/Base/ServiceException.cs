using System;
using System.Collections.Generic;

namespace Core.Services.Base
{
    // 500
    public class ServiceException : Exception
    {
        public ServiceException(Exception inner) : this(inner.Message, inner)
        {
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    // 404
    public class DataNotFoundException : ServiceException
    {
        public DataNotFoundException(string message) : base(message)
        {
        }
    }

    // 409
    // The client will display error notify with message from this exception
    // Example use case: Delete item that have relation => conflict 
    public class ConflictException : ServiceException
    {
        public ConflictException(string message) : base(message)
        {
        }
    }


    // 403
    public class ForbiddenException : ServiceException
    {
        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException() : base("You don't have permission to do this")
        {
        }
    }

    // 400
    public class InvalidDataException : ServiceException
    {
        public IDictionary<string, ICollection<string>> ModelErrors { get; }

        public InvalidDataException(string field, params string[] messages) : base("Model is invalid")
        {
            ModelErrors = new Dictionary<string, ICollection<string>>
            {
                [field] = messages
            };
        }

        public InvalidDataException() : base("Model is invalid")
        {
            ModelErrors = new Dictionary<string, ICollection<string>>();
        }

        public ICollection<string> this[string index]
        {
            set => ModelErrors[index] = value;
            get => ModelErrors[index];
        }
    }
}