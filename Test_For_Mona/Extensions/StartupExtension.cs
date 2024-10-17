using AutoMapper;
using Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services.Core;
using Services.Mapping;
using System.Reflection;
using Newtonsoft.Json.Converters;

namespace Test_For_Mona.Extensions;

public static class StartupExtension
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Dev"),
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        });
    }

    public static void ApplyPendingMigrations(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapperProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }

    public static void AddBussinessService(this IServiceCollection services)
    {
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API Test For Mona",
                Version = "1.0",
                Description = "Test For Mona",
            });
            c.UseInlineDefinitionsForEnums();

        });

        services.AddControllersWithViews().AddNewtonsoftJson(options =>
        options.SerializerSettings.Converters.Add(new StringEnumConverter()));
    }
}
