using Asp.Versioning;
using Vehicles.Application;
using Vehicles.Infrastructure;

namespace Vehicles.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddProblemDetails();
        builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        ConfigureServices(builder.Services);

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        if (!app.Environment.IsDevelopment())
        {
            // In non development environments, don't show the stack trace. It's good practice to not leak details like this
            // to the outside world in non-development environments
            app.UseExceptionHandler(exceptionPipeline =>
            {
                exceptionPipeline.Run(async context => await Results.Problem().ExecuteAsync(context));
            });
        }

        app.MapControllers();
        app.Run();
    }

    private static IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddApplication()
            .AddInfrastructure();
    }
}


