namespace PsyAssistPlatform.Messages;

public class FeedbackMessage
{
    public int Id { get; set; }

    public required string Telegram { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public required string FeedbackText { get; set; }

    public int? QuestionnaireNumber { get; set; }
}
