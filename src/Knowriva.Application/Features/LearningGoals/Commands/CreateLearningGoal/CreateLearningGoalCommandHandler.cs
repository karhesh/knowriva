using Knowriva.Application.Features.LearningGoals.Dtos;
using Knowriva.Domain.Entities.LearningGoals;
using MediatR;

namespace Knowriva.Application.Features.LearningGoals.Commands.CreateLearningGoal;

public sealed class CreateLearningGoalCommandHandler : IRequestHandler<CreateLearningGoalCommand, LearningGoalDto>
{
    public Task<LearningGoalDto> Handle(CreateLearningGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = LearningGoal.Create(
            request.Title,
            request.Description,
            request.TargetDate);

        var dto = new LearningGoalDto(
           goal.Id,
           goal.Title,
           goal.Description,
           goal.TargetDate,
           goal.CreatedAtUtc,
           goal.UpdatedAtUtc);

        return Task.FromResult(dto);
    }
}