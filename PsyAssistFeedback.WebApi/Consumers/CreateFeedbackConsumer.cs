using AutoMapper;
using MassTransit;
using PsyAssistFeedback.Application.Dto.Feedback;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistPlatform.Messages;
using Serilog;

namespace PsyAssistFeedback.WebApi.Consumers;

public class CreateFeedbackConsumer :
    IConsumer<FeedbackMessage>
{
    private readonly IFeedbackService _feedbackService;
    private readonly IMapper _mapper;

    public CreateFeedbackConsumer(
        IFeedbackService feedbackService, 
        IMapper mapper)
    {
        _feedbackService = feedbackService;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<FeedbackMessage> context)
    {
        Log.Information("Message from telegram bot consumed");

        var feedback = _mapper.Map<CreateFeedbackDto>(context.Message);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        await _feedbackService.CreateFeedbackAsync(feedback, cancellationToken);
    }
}
