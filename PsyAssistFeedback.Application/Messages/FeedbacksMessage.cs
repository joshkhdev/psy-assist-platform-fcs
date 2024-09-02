namespace PsyAssistPlatform.Messages;

public class FeedbacksMessage
{
    public required IEnumerable<FeedbackMessage> Items { get; set; }
}
