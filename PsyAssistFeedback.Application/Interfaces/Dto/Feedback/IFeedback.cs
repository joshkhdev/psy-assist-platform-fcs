namespace PsyAssistFeedback.Application.Interfaces.Dto.Feedback;

public interface IFeedback
{
    string Telegram { get; set; }

    DateTime? FeedbackDate { get; set; }

    string FeedbackText { get; set; }

    int? QuestionnaireNumber { get; set; }
}