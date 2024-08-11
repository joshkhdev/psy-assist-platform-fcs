namespace PsyAssistFeedback.Application.Interfaces.Dto.Feedback;

public interface IFeedback
{
    int Id { get; set; }

    string Telegram { get; set; }

    DateTime? FeedbackDate { get; set; }

    string FeedbackText { get; set; }

    int? QuestionnaireNumber { get; set; }
}