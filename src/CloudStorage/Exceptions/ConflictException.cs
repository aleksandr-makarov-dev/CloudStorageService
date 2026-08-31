using System.Net;

namespace CloudStorage.Exceptions;

public sealed class ConflictException(string message, Exception? innerException = null)
    : ApplicationException(message, innerException, HttpStatusCode.Conflict);