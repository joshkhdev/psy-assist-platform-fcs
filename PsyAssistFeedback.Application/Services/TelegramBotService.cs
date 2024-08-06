using MassTransit;
using Microsoft.Extensions.Configuration;
using PsyAssistFeedback.Application.Dto.Feedback;
using PsyAssistFeedback.Application.Exceptions;
using PsyAssistFeedback.Application.Interfaces.Service;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace PsyAssistFeedback.Application.Services;

public class TelegramBotService : ITelegramBotService
{
    private const string HELLO_MESSAGE = "������ ����! ����� �������� �����, �������, ����������, � ����� ��������� ����� ������ (�� �����������, ���� ��� ������� � ��������� ��������� � ��������� � ������) � ����� ������...";

    private readonly IConfiguration _configuration;
    private readonly IBus _bus;

    private ITelegramBotClient? _bot;

    public TelegramBotService(IConfiguration configuration, IBus bus)
    {
        _bus = bus;
        _configuration = configuration;

        InitializeBot();
    }

    public void InitializeBot()
    {
        var token = _configuration["TelegramBot:Token"];

        if (string.IsNullOrEmpty(token))
        {
            throw new TelegramBotEmptyTokenException(
                "Telegram bot token is empty or does not exist in user secrets");
        }

        _bot = new TelegramBotClient(token);
    }

    public void RunBot()
    {
        try
        {
            if (_bot == null)
            {
                throw new TelegramBotInitializationException(
                    "Telegram bot is not initialized");
            }

            Console.WriteLine("������� ��� " + _bot.GetMeAsync().Result.FirstName);

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
            throw;
            // TODO: �������� ����������� � ��������� ������
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
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

        await botClient.SendTextMessageAsync(message.Chat, $"�������, {update.Message.From.Username}, ��� ����� ���������!");
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
    }
}