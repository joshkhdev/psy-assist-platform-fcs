using PsyAssistFeedback.Application.Interfaces.Dto.Feedback;

namespace PsyAssistFeedback.Application.Dto.Feedback;

public record CreateFeedbackDto : ICreateFeedback
{
    public required string Telegram { get; set; }

    public required string FeedbackText { get; set; }

    public int? QuestionnaireNumber { get; set; }
}