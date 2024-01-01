
using MassTransit;
using MassTransit.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ConsumerApi;

public class Program
{
    public static void Main(string[] args)
    {
        const string serviceName = "consumer-service";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMassTransit(config =>
        {
            config.AddConsumer<MessageConsumer>();

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("rabbitmq://localhost");
                cfg.ConfigureEndpoints(ctx);
            });
        });


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