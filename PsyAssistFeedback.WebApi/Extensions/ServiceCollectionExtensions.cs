using MassTransit;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistFeedback.Application.Services;
using PsyAssistFeedback.WebApi.Consumers;

namespace PsyAssistFeedback.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration)
            => services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateFeedbackConsumer>();
                var serviceAddress = configuration["IntegrationSettings:RabbitFeedbackServiceUrl"];
                var serviceName = new Uri(serviceAddress).Segments.LastOrDefault();
                x.AddConsumer<GetFeedbacksConsumer>().Endpoint(e => e.Name = serviceName);

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitUrl = configuration["IntegrationSettings:RabbitUrl"];

                    cfg.Host(rabbitUrl, h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

        public static IServiceCollection AddTelegramBot(this IServiceCollection services)
            => services.AddSingleton<IFeedbackMessagesService, FeedbackMessagesService>()
                .AddSingleton<ITelegramBotService, TelegramBotService>();
    }
}
