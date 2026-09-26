using Knowriva.Application.Common.Interfaces;
using Knowriva.Domain.Entities.LearningGoals;
using Microsoft.EntityFrameworkCore;

namespace Knowriva.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IAppDbContext
{
    public DbSet<LearningGoal> LearningGoals => Set<LearningGoal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
