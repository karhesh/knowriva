using System.Net;
using System.Net.Http.Json;
using Knowriva.Application.Features.LearningGoals.Dtos;
using Knowriva.Contracts.Requests.LearningGoals;

namespace Knowriva.Api.Tests;

public sealed class LearningGoalsApiTests
{
    [Fact]
    public async Task GetLearningGoals_WhenNoGoalsExist_ReturnsEmptyList()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/learning-goals");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("[]", await response.Content.ReadAsStringAsync());
    }
    [Fact]
    public async Task CreateLearningGoal_WhenRequestIsValid_ReturnsCreatedAndCanBeRetrieved()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var request = new CreateLearningGoalRequest
        {
            Title = "Learn integration testing",
        };

        var createResponse = await client.PostAsJsonAsync("/api/learning-goals", request);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.NotNull(createResponse.Headers.Location);

        var createdGoal = await createResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(createdGoal);
        Assert.Equal(request.Title, createdGoal.Title);

        var getResponse = await client.GetAsync(createResponse.Headers.Location);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrievedGoal = await getResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(retrievedGoal);
        Assert.Equal(createdGoal.Id, retrievedGoal.Id);
        Assert.Equal(request.Title, retrievedGoal.Title);
    }
    [Fact]
    public async Task CreateLearningGoal_WhenTitleIsEmpty_ReturnsValidationProblem()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var request = new CreateLearningGoalRequest
        {
            Title = ""
        };
        var response = await client.PostAsJsonAsync("/api/learning-goals", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Title is required", await response.Content.ReadAsStringAsync());

        var getResponse = await client.GetAsync("/api/learning-goals");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.Equal("[]", await getResponse.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task CreateLearningGoal_WhenValid_AppearsInList()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var request = new CreateLearningGoalRequest
        {
            Title = "Study EF Core"
        };

        var createResponse = await client.PostAsJsonAsync("/api/learning-goals", request);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdGoal = await createResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(createdGoal);

        var listResponse = await client.GetAsync("/api/learning-goals");
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        var goals = await listResponse.Content.ReadFromJsonAsync<List<LearningGoalDto>>();
        Assert.NotNull(goals);

        var listedGoal = Assert.Single(goals);
        Assert.Equal(createdGoal.Id, listedGoal.Id);
        Assert.Equal(request.Title, listedGoal.Title);
    }

    [Fact]
    public async Task GetLearningGoal_WhenIdDoesNotExist_ReturnsNotFound()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var missingId = Guid.NewGuid();
        var response = await client.GetAsync($"/api/learning-goals/{missingId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}
