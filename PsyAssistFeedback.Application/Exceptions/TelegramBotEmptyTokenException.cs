namespace PsyAssistFeedback.Application.Exceptions
{
    public class TelegramBotEmptyTokenException : Exception
    {
        public TelegramBotEmptyTokenException(string message) : base(message)
        {
        }
    }
}
