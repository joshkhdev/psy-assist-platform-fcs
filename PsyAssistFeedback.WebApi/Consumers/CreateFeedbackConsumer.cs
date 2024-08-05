using MassTransit;
using PsyAssistFeedback.Application.Dto.Feedback;
using PsyAssistFeedback.Application.Interfaces.Service;

namespace PsyAssistFeedback.WebApi.Consumers
{
    public class CreateFeedbackConsumer :
        IConsumer<CreateFeedbackDto>
    {
        private readonly IFeedbackService _FeedbackService;

        public CreateFeedbackConsumer(IFeedbackService FeedbackService, ILogger<CreateFeedbackConsumer> logger)
        {
            _FeedbackService = FeedbackService;
        }

        public async Task Consume(ConsumeContext<CreateFeedbackDto> context)
        {
            await _FeedbackService.CreateFeedbackAsync(context.Message, CancellationToken.None);
        }
    }
}
