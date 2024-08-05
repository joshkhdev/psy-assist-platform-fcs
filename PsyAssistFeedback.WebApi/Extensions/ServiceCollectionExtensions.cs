using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistFeedback.Application.Services;

namespace PsyAssistFeedback.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services)
        => services.AddSingleton<TelegramBotService>();

    public static void RunTelegramBot(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        var fooService = sp.GetService<ITelegramBotService>();

        fooService?.Run();
    }
}
