using Knowriva.Application.Common.Interfaces;
using Knowriva.Application.Features.LearningGoals.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Knowriva.Application.Features.LearningGoals.Queries.GetLearningGoals;

public sealed class GetLearningGoalsQueryHandler(IAppDbContext context)
: IRequestHandler<GetLearningGoalsQuery, IReadOnlyList<LearningGoalDto>>
{
    public async Task<IReadOnlyList<LearningGoalDto>> Handle(GetLearningGoalsQuery request, CancellationToken cancellationToken)
    {
        var goals = await context.LearningGoals
        .AsNoTracking()
        .OrderBy(goal => goal.Id)
        .Select(goal => new LearningGoalDto(
            goal.Id,
            goal.Title,
            goal.Description,
            goal.TargetDate,
            goal.CreatedAtUtc,
            goal.UpdatedAtUtc)).ToListAsync(cancellationToken);
        return goals;
    }
}