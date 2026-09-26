using Knowriva.Application.Features.LearningGoals.Commands.CreateLearningGoal;
using Knowriva.Application.Features.LearningGoals.Dtos;
using Knowriva.Application.Features.LearningGoals.Queries.GetLearningGoalById;
using Knowriva.Application.Features.LearningGoals.Queries.GetLearningGoals;
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
        return CreatedAtRoute("GetLearningGoalById", new { Id = result.Id }, result);
    }
    [HttpGet]
    public async Task<IActionResult> GetLearningGoals(CancellationToken ct)
    {
        var result = await sender.Send(new GetLearningGoalsQuery(), ct);
        return Ok(result);
    }
    [HttpGet("{Id:guid}", Name = "GetLearningGoalById")]
    public async Task<IActionResult> GetLearningGoal(Guid Id, CancellationToken ct)
    {
        var result = await sender.Send(new GetLearningGoalByIdQuery(Id), ct);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }
}
