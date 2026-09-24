using Knowriva.Domain.Common;
namespace Knowriva.Domain.Entities.LearningGoals;

public sealed class LearningGoal : Entity
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateOnly? TargetDate { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? UpdatedAtUtc { get; private set; }

    private LearningGoal(string title, string? description, DateOnly? targetDate)
    {
        Title = title;
        Description = description;
        TargetDate = targetDate;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public static LearningGoal Create(string title, string? description, DateOnly? targetDate)
    {
        return new LearningGoal(title, description, targetDate);
    }

}