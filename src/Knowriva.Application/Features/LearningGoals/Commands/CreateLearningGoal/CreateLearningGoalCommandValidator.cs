using FluentValidation;
namespace Knowriva.Application.Features.LearningGoals.Commands.CreateLearningGoal;

public sealed class CreateLearningGoalCommandValidator : AbstractValidator<CreateLearningGoalCommand>
{
    public CreateLearningGoalCommandValidator()
    {
        RuleFor(x => x.Title)
        .NotEmpty().WithMessage("Title is required")
        .MaximumLength(200);

        RuleFor(x => x.Description)
        .MaximumLength(1000);


    }
}