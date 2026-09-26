namespace Knowriva.Contracts.Requests.LearningGoals;

public class CreateLearningGoalRequest
{
    public string Title {get; set;} = string.Empty;
    public string? Description {get; set;}
    public DateOnly? TargetDate {get; set;}
}