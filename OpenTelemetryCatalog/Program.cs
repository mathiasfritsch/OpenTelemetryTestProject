using MassTransit;
using MassTransit.Logging;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenTelemetryCatalog;

public class Program
{
    public static void Main(string[] args)
    {
        const string serviceName = "producer-service";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("rabbitmq://localhost");
                cfg.ConfigureEndpoints(ctx);
            });
        });
        builder.Services.AddDbContext<CatalogContext>(
            );

        builder.Logging.AddOpenTelemetry(options =>
        {
            options
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName))
                .AddConsoleExporter();
        });



        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                .AddNpgsql()
                .AddConsoleExporter()
                .AddOtlpExporter()
            )
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter());


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}