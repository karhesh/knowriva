using Knowriva.Application.Features.LearningGoals.Dtos;
using MediatR;

namespace Knowriva.Application.Features.LearningGoals.Queries.GetLearningGoals;

public sealed record GetLearningGoalsQuery : IRequest<IReadOnlyList<LearningGoalDto>>;