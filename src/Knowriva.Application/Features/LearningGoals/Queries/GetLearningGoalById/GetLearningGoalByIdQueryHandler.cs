using Knowriva.Application.Common.Interfaces;
using Knowriva.Application.Features.LearningGoals.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Knowriva.Application.Features.LearningGoals.Queries.GetLearningGoalById;

public sealed class GetLearningGoalByIdQueryHandler(IAppDbContext context)
: IRequestHandler<GetLearningGoalByIdQuery, LearningGoalDto?>
{
    private readonly IAppDbContext _context = context;

    public async Task<LearningGoalDto?> Handle(GetLearningGoalByIdQuery request, CancellationToken cancellationToken)
    {
        var goal = await _context.LearningGoals
        .AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (goal is null)
        {
            return null;
        }
        var dto = new LearningGoalDto(
           goal.Id,
           goal.Title,
           goal.Description,
           goal.TargetDate,
           goal.CreatedAtUtc,
           goal.UpdatedAtUtc);
        return dto;
    }
}