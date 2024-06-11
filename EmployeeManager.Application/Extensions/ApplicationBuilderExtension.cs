using EmployeeManager.Infrastructure;

namespace EmployeeManager.Application.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication SeedData(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            seeder.Seed();
        }
        return app;
    }
}