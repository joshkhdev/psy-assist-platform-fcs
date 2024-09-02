using PsyAssistPlatform.Domain;

namespace PsyAssistFeedback.Persistence.Data;

public static class FakeDataFactory
{
    public static IEnumerable<Feedback> Feedbacks => new List<Feedback>()
    {
        new()
        {
            Telegram = "@user1",
            FeedbackDate = DateTime.UtcNow,
            FeedbackText = "Test 1"
        },
        new()
        {
            Telegram = "@user2",
            FeedbackDate = DateTime.UtcNow,
            FeedbackText = "Test 2"
        },
        new()
        {
            Telegram = "@user3",
            FeedbackDate = DateTime.UtcNow,
            FeedbackText = "Test 3"
        }
    };
}