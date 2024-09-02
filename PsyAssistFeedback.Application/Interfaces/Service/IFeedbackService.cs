using PsyAssistFeedback.Application.Interfaces.Dto.Feedback;

namespace PsyAssistFeedback.Application.Interfaces.Service;

public interface IFeedbackService
{
    Task<IEnumerable<IFeedback>?> GetFeedbacksAsync(CancellationToken cancellationToken);

    Task<IFeedback?> GetFeedbackByIdAsync(int id, CancellationToken cancellationToken);

    Task<IFeedback> CreateFeedbackAsync(ICreateFeedback FeedbackData, CancellationToken cancellationToken);
}