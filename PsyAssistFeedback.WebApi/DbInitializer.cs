using PsyAssistFeedback.Persistence;
using PsyAssistFeedback.Persistence.Data;

namespace PsyAssistFeedback.WebApi;

public static class DbInitializer
{
    public static void Initialize(PsyAssistContext context)
    {
        InitializeDatabase(context);
        AddFakeData(context);
    }

    private static void InitializeDatabase(PsyAssistContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    private static void AddFakeData(PsyAssistContext context)
    {
        context.Feedbacks.AddRange(FakeDataFactory.Feedbacks);
        context.SaveChanges();
    }
}