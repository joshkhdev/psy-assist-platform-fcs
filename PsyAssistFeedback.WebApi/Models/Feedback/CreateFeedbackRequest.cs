using PsyAssistFeedback.Application.Interfaces.Dto.Feedback;

namespace PsyAssistFeedback.WebApi.Models.Feedback;

public record CreateFeedbackRequest : ICreateFeedback
{
    public required string Telegram { get; set; }

    public required string FeedbackText { get; set; }

    public int? QuestionnaireNumber { get; set; }
}
