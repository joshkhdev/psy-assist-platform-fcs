namespace PsyAssistFeedback.WebApi.Models.Feedback;

public record FeedbackResponse
{
    public string Telegram { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public string FeedbackText { get; set; }

    public int? QuestionnaireNumber { get; set; }
}