using Knowriva.Domain.Entities.LearningGoals;
using Microsoft.EntityFrameworkCore;

namespace Knowriva.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<LearningGoal> LearningGoals { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
