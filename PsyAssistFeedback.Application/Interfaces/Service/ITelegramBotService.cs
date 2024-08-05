using Telegram.Bot.Types;
using Telegram.Bot;

namespace PsyAssistFeedback.Application.Interfaces.Service
{
    public interface ITelegramBotService
    {
        void Run();

        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}
