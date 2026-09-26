using Knowriva.Application.Features.LearningGoals.Dtos;
using MediatR;

namespace Knowriva.Application.Features.LearningGoals.Commands.CreateLearningGoal;

public sealed record CreateLearningGoalCommand(
    string Title,
    string? Description,
    DateOnly? TargetDate) : IRequest<LearningGoalDto>;