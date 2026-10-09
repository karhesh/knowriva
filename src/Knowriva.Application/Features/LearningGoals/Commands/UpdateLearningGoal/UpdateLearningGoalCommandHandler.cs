using Knowriva.Application.Common.Interfaces;
using Knowriva.Application.Features.LearningGoals.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Knowriva.Application.Features.LearningGoals.Commands.UpdateLearningGoal;

public sealed class UpdateLearningGoalCommandHandler(IAppDbContext context)
: IRequestHandler<UpdateLearningGoalCommand, LearningGoalDto?>
{
    public async Task<LearningGoalDto?> Handle(UpdateLearningGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = await context.LearningGoals.FirstOrDefaultAsync
        (goal => goal.Id == request.Id, cancellationToken);

        if (goal is null)
        {
            return null;
        }

        goal.UpdateDetails(request.Title, request.Description, request.TargetDate);

        await context.SaveChangesAsync(cancellationToken);

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