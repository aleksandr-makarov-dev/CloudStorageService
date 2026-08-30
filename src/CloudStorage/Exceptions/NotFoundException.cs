namespace CloudStorage.Exceptions;

public class NotFoundException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);