using Microsoft.Data.Sqlite;
using Knowriva.Application.Common.Interfaces;
using Knowriva.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Knowriva.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string contentRootPath)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("The DefaultConnection connection string is missing.");

        var sqliteConnection = new SqliteConnectionStringBuilder(connectionString);
        sqliteConnection.DataSource = Path.GetFullPath(sqliteConnection.DataSource, contentRootPath);

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(sqliteConnection.ConnectionString));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        return services;
    }
}
