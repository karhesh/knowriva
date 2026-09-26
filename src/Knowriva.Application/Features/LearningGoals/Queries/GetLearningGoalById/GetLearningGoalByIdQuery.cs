using Knowriva.Application.Features.LearningGoals.Dtos;
using MediatR;

namespace Knowriva.Application.Features.LearningGoals.Queries.GetLearningGoalById;

public sealed record GetLearningGoalByIdQuery(Guid Id)
                    : IRequest<LearningGoalDto?>;