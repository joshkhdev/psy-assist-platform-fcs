using MassTransit;
using PsyAssistFeedback.Application.Dto.Feedback;
using PsyAssistFeedback.Application.Interfaces.Service;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace PsyAssistFeedback.Application.Services;

public class TelegramBotService : ITelegramBotService
{
    private const string BOT_TOKEN = "7140894509:AAHoEOREZL9cBZdwwZ7FG5EvcrBEIzj8hQY";
    private const string HELLO_MESSAGE = "Добрый день! Чтобы оставить отзыв, введите, пожалуйста, в одном сообщении номер заявки (не обязательно, если ваш никнейм в Телеграме совпадает с указанным в анкете) и текст отзыва...";

    private readonly ITelegramBotClient _bot;
    private readonly IBus _bus;

    public TelegramBotService(IBus bus)
    {
        _bot = new TelegramBotClient(BOT_TOKEN);
        _bus = bus;
    }

    public void Run()
    {
        try
        {
            Console.WriteLine("Запущен бот " + _bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            _bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            // TODO: Добавить логирование и обработку ошибок
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

        if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message)
            return;

        var message = update.Message;
        if (string.IsNullOrEmpty(message?.Text))
            return;

        if (message.Text.Equals("/start", StringComparison.CurrentCultureIgnoreCase))
        {
            await botClient.SendTextMessageAsync(message.Chat, HELLO_MESSAGE);
            return;
        }

        await PublishFeedbackAsync(message, update, cancellationToken);

        await botClient.SendTextMessageAsync(message.Chat, $"Спасибо, {update.Message.From.Username}, Ваш отзыв отправлен!");
    }

    public async Task PublishFeedbackAsync(Message message, Update update, CancellationToken cancellationToken)
    {
        var request = new CreateFeedbackDto
        {
            Telegram = update.Message.From.Username,
            FeedbackText = message.Text
        };
        await _bus.Publish(request, cancellationToken);
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }
}