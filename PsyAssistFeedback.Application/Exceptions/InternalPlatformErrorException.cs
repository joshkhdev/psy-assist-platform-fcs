namespace PsyAssistFeedback.Application.Exceptions;

public class InternalPlatformErrorException : Exception
{
    public InternalPlatformErrorException(string message) : base(message)
    {
    }
}