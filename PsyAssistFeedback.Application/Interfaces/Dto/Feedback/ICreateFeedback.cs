namespace PsyAssistFeedback.Application.Interfaces.Dto.Feedback;

public interface ICreateFeedback
{
    string Telegram { get; set; }

    string FeedbackText { get; set; }
}