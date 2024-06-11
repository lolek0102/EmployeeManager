using EmployeeManager.Domain.Repositories;
using EmployeeManager.Infrastructure;
using EmployeeManager.Infrastructure.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Application.Extensions;

public static class InfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(configuration["Database:Name"]!));
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<DataSeeder>();
        return services;
    }
}