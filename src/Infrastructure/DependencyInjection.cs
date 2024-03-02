using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Domain.Applications;
using Domain.Configurations;
using Domain.ExchangePrograms;
using Domain.GenericStatuses;
using Domain.Organizations;
using Domain.Users;
using Infrastructure.Abstractions.Authentication;
using Infrastructure.Abstractions.Email;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPersistence(services, configuration);
        AddAuthentication(services);
        AddEmailSender(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        #region DbContext

        var connectionString =
            configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
            x => x.MigrationsAssembly("Migrations"));
        });

        #endregion

        #region Dapper

        services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));

        #endregion

        #region Repositories Registration

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IExchangeProgramRepository, ExchangeProgramRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IGenericStatusRepository, GenericStatusRepository>();
        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();

        #endregion

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
    }

    private static void AddEmailSender(IServiceCollection services)
    {
        services.AddScoped<IEmailSender, EmailSender>();
    }
}
