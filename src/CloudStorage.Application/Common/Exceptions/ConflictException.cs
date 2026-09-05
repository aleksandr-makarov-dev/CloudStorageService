using System.Net;

namespace CloudStorage.Application.Common.Exceptions;

public sealed class ConflictException(string message, Exception? innerException = null)
    : ApplicationException(message, innerException, HttpStatusCode.Conflict);