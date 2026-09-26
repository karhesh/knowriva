namespace Knowriva.Application.Features.LearningGoals.Dtos;

public sealed record LearningGoalDto(
    Guid Id,
    string Title,
    string? Description,
    DateOnly? TargetDate,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);