using Knowriva.Application.Common.Interfaces;
using Knowriva.Application.Features.LearningGoals.Dtos;
using Knowriva.Domain.Entities.LearningGoals;
using MediatR;

namespace Knowriva.Application.Features.LearningGoals.Commands.CreateLearningGoal;

public sealed class CreateLearningGoalCommandHandler(IAppDbContext context)
    : IRequestHandler<CreateLearningGoalCommand, LearningGoalDto>
{
    public async Task<LearningGoalDto> Handle(CreateLearningGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = LearningGoal.Create(
            request.Title,
            request.Description,
            request.TargetDate);

        context.LearningGoals.Add(goal);
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
