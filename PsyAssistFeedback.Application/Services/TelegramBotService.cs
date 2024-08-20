using Microsoft.Extensions.Configuration;
using PsyAssistFeedback.Application.Exceptions;
using PsyAssistFeedback.Application.Interfaces.Service;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace PsyAssistFeedback.Application.Services;

public class TelegramBotService : ITelegramBotService
{
    private const string HelloMessage = "Добрый день! Чтобы оставить отзыв, введите, пожалуйста, в одном сообщении номер заявки (не обязательно, если ваш никнейм в Телеграме совпадает с указанным в анкете) и текст отзыва...";
    private const string TelergamSuccessMesage = "Спасибо, {0}, Ваш отзыв отправлен!";
    private const string EmptyTokenMessage = "Telegram bot token is empty or does not exist in user secrets";
    private const string InitializationErrorMessage = "Telegram bot is not initialized";
    private const string EmptyUsernameMessage = "Username cannot be empty";
    private const string EmptyTextMessage = "Feedback text cannot be null or empty";

    private readonly IConfiguration _configuration;
    private readonly IFeedbackMessagesService _feedbackMessagesService;

    private ITelegramBotClient? _bot;

    public TelegramBotService(
        IConfiguration configuration, 
        IFeedbackMessagesService feedbackMessagesService)
    {
        _configuration = configuration;
        _feedbackMessagesService = feedbackMessagesService;

        InitializeBot();
    }

    public void InitializeBot()
    {
        var token = _configuration["TelegramBot:Token"];

        if (string.IsNullOrEmpty(token))
            throw new TelegramBotEmptyTokenException(EmptyTokenMessage);

        _bot = new TelegramBotClient(token);
    }

    public void RunBot()
    {
        if (_bot is null)
            throw new TelegramBotInitializationException(InitializationErrorMessage);

        Log.Information($"Start telegram bot {_bot.GetMeAsync().Result.FirstName}");

        var cancellationToken = new CancellationTokenSource().Token;
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

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message)
            return;

        var message = update.Message;
        if (string.IsNullOrWhiteSpace(message?.Text))
            throw new IncorrectDataException(EmptyUsernameMessage);
        if (string.IsNullOrWhiteSpace(message?.From?.Username))
            throw new IncorrectDataException(EmptyTextMessage);

        if (message.Text.Equals("/start", StringComparison.CurrentCultureIgnoreCase))
        {
            await botClient.SendTextMessageAsync(message.Chat, HelloMessage);
            return;
        }

        Log.Information($"Sending message from telegram bot to service: username - {message.From.Username}");
        await _feedbackMessagesService.PublishFeedbackAsync(message, cancellationToken);

        await botClient.SendTextMessageAsync(message.Chat, string.Format(TelergamSuccessMesage, message.From.Username));
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) 
        => Task.CompletedTask;
}