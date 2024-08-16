namespace PsyAssistFeedback.Application.Exceptions;

public class TelegramBotInitializationException : Exception
{
    public TelegramBotInitializationException(string message) : base(message)
    {
    }
}
