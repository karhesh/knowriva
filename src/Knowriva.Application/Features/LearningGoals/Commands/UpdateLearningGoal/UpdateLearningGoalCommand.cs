using Knowriva.Application.Features.LearningGoals.Dtos;
using MediatR;

namespace Knowriva.Application.Features.LearningGoals.Commands.UpdateLearningGoal;

public sealed record UpdateLearningGoalCommand
(Guid Id, string Title, string? Description, DateOnly? TargetDate)
: IRequest<LearningGoalDto?>;