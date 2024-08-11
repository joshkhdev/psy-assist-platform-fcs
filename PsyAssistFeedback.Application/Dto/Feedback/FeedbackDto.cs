using PsyAssistFeedback.Application.Interfaces.Dto.Feedback;

namespace PsyAssistFeedback.Application.Dto.Feedback;

public record FeedbackDto : IFeedback
{
    public int Id { get; set; }

    public required string Telegram { get; set; }

    public required DateTime? FeedbackDate { get; set; }

    public required string FeedbackText { get; set; }

    public int? QuestionnaireNumber { get; set; }
}