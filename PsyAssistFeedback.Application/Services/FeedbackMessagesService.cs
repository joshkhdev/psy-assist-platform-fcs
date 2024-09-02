using MassTransit;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistPlatform.Messages;
using Telegram.Bot.Types;

namespace PsyAssistFeedback.Application.Services;

public class FeedbackMessagesService : IFeedbackMessagesService
{
    private readonly IBus _bus;

    public FeedbackMessagesService(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishFeedbackAsync(Message message, CancellationToken cancellationToken)
    {
        var request = new FeedbackMessage
        {
            Telegram = message.From!.Username!,
            FeedbackText = message.Text!
        };

        await _bus.Publish(request, cancellationToken);
    }
}
