using Knowriva.Application.Features.LearningGoals.Commands.CreateLearningGoal;
using Knowriva.Contracts.Requests.LearningGoals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Knowriva.Api.Controllers;

[ApiController]
[Route("api/learning-goals")]
public sealed class LearningGoalsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLearningGoal([FromBody] CreateLearningGoalRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CreateLearningGoalCommand(request.Title, request.Description, request.TargetDate), ct);
        return Ok(result);
    }
}