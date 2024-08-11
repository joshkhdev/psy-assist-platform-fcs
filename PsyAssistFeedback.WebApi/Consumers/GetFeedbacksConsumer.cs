using AutoMapper;
using MassTransit;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistPlatform.Messages;

namespace PsyAssistFeedback.WebApi.Consumers
{
    public class GetFeedbacksConsumer :
        IConsumer<FeedbacksMessage>
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;

        public GetFeedbacksConsumer(
            IFeedbackService feedbackService, 
            IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<FeedbacksMessage> context)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var feedbacks = await _feedbackService.GetFeedbacksAsync(cancellationToken);
            var feedbackMessage = new FeedbacksMessage 
            { 
                Items = _mapper.Map<IEnumerable<FeedbackMessage>>(feedbacks) 
            };

            await context.RespondAsync(feedbackMessage);
        }
    }
}
