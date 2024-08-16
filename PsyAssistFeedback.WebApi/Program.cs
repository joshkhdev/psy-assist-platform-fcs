using PsyAssistFeedback.Persistence;
using Serilog;
using Serilog.Events;

namespace PsyAssistFeedback.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.File(
                $"{Environment.CurrentDirectory}/Logs/PsyAssistPlatformFeedbackWebApiLog-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30
            ).CreateLogger();

        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        try
        {
            var context = serviceProvider.GetRequiredService<PsyAssistContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error ocurred while app initialization");
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}