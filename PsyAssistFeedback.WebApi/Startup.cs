using MassTransit;
using Microsoft.EntityFrameworkCore;
using PsyAssistFeedback.Application.Interfaces.Repository;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistFeedback.Application.Mapping;
using PsyAssistFeedback.Application.Services;
using PsyAssistFeedback.Persistence;
using PsyAssistFeedback.Persistence.Repositories;
using PsyAssistFeedback.WebApi.Consumers;
using PsyAssistFeedback.WebApi.Mapping;
using PsyAssistFeedback.WebApi.Middlewares;

namespace PsyAssistFeedback.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationMappingProfile), typeof(PresentationMappingProfile));
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
        services.AddScoped<IFeedbackService, FeedbackService>();

        services.AddDbContext<PsyAssistContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            options.UseLazyLoadingProxies();
            options.EnableSensitiveDataLogging();
        });

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateFeedbackConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitUrl = Configuration["IntegrationSettings:RabbitUrl"];

                cfg.Host(rabbitUrl, h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddSingleton<ITelegramBotService, TelegramBotService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITelegramBotService telegramBotService)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        telegramBotService.Run();
    }
}