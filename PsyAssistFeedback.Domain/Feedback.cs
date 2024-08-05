using PsyAssistFeedback.Domain;

namespace PsyAssistPlatform.Domain;

public class Feedback : BaseEntity
{
    public string Telegram { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public string FeedbackText { get; set; }

    public int? QuestionnaireNumber { get; set; }
}