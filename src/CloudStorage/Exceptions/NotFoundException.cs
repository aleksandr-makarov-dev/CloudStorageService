using System.Net;

namespace CloudStorage.Exceptions;

public sealed class NotFoundException(string resourceName, object resourceId, Exception? innerException = null)
    : ApplicationException($"{resourceName} with id '{resourceId}' was not found.",
        innerException, HttpStatusCode.NotFound);