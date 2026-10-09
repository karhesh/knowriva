using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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
    [Fact]
    public async Task UpdateLearningGoal_WhenRequestIsValid_UpdatesAndPersistsDetails()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var createRequest = new CreateLearningGoalRequest
        {
            Title = "Learn EF Core",
            Description = "Study basic queries",
            TargetDate = new DateOnly(2026, 12, 31)
        };

        using var createResponse = await client.PostAsJsonAsync("/api/learning-goals", createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdGoal = await createResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(createdGoal);

        var updateRequest = new UpdateLearningGoalRequest
        {
            Title = "Practise EF Core",
            Description = "Build and test database queries",
            TargetDate = new DateOnly(2027, 1, 31)
        };

        var goalUrl = $"/api/learning-goals/{createdGoal.Id}";
        using var updateResponse = await client.PutAsJsonAsync(goalUrl, updateRequest);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedGoal = await updateResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(updatedGoal);
        Assert.Equal(updateRequest.Title, updatedGoal.Title);
        Assert.Equal(updateRequest.Description, updatedGoal.Description);
        Assert.Equal(updateRequest.TargetDate, updatedGoal.TargetDate);
        Assert.Equal(createdGoal.Id, updatedGoal.Id);
        Assert.Equal(createdGoal.CreatedAtUtc, updatedGoal.CreatedAtUtc);
        Assert.NotNull(updatedGoal.UpdatedAtUtc);

        using var getResponse = await client.GetAsync(goalUrl);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrievedGoal = await getResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(retrievedGoal);
        Assert.Equal(updatedGoal, retrievedGoal);
    }

    [Fact]
    public async Task UpdateLearningGoal_WhenTitleIsEmpty_ReturnsValidationProblemAndLeavesGoalUnchanged()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var createRequest = new CreateLearningGoalRequest
        {
            Title = "Learn EF Core",
            Description = "Study basic queries",
            TargetDate = new DateOnly(2026, 12, 31)
        };

        using var createResponse = await client.PostAsJsonAsync("/api/learning-goals", createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdGoal = await createResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(createdGoal);

        var updateRequest = new UpdateLearningGoalRequest
        {
            Title = "",
            Description = "This change must not be saved",
            TargetDate = new DateOnly(2027, 1, 31)
        };

        var goalUrl = $"/api/learning-goals/{createdGoal.Id}";
        using var updateResponse = await client.PutAsJsonAsync(goalUrl, updateRequest);
        Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

        var problem = await updateResponse.Content.ReadFromJsonAsync<JsonElement>();
        var titleErrors = problem.GetProperty("errors").GetProperty("Title");
        Assert.Contains("Title is required", titleErrors.EnumerateArray().Select(error => error.GetString()));

        using var getResponse = await client.GetAsync(goalUrl);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrievedGoal = await getResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(retrievedGoal);
        Assert.Equal(createdGoal, retrievedGoal);
    }

    [Theory]
    [InlineData(201, 10, "Title")]
    [InlineData(10, 1001, "Description")]
    public async Task UpdateLearningGoal_WhenTextExceedsLimit_ReturnsValidationProblemAndLeavesGoalUnchanged(
        int titleLength, int descriptionLength, string errorField)
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var createRequest = new CreateLearningGoalRequest
        {
            Title = "Learn EF Core",
            Description = "Study basic queries",
            TargetDate = new DateOnly(2026, 12, 31)
        };

        using var createResponse = await client.PostAsJsonAsync("/api/learning-goals", createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdGoal = await createResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(createdGoal);

        var updateRequest = new UpdateLearningGoalRequest
        {
            Title = new string('T', titleLength),
            Description = new string('D', descriptionLength),
            TargetDate = new DateOnly(2027, 1, 31)
        };

        var goalUrl = $"/api/learning-goals/{createdGoal.Id}";
        using var updateResponse = await client.PutAsJsonAsync(goalUrl, updateRequest);
        Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

        var problem = await updateResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(problem.GetProperty("errors").TryGetProperty(errorField, out _));

        using var getResponse = await client.GetAsync(goalUrl);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrievedGoal = await getResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(retrievedGoal);
        Assert.Equal(createdGoal, retrievedGoal);
    }

    [Fact]
    public async Task UpdateLearningGoal_WhenIdDoesNotExist_ReturnsNotFound()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var updateRequest = new UpdateLearningGoalRequest
        {
            Title = "Learn EF Core"
        };

        var missingId = Guid.NewGuid();
        using var response = await client.PutAsJsonAsync($"/api/learning-goals/{missingId}", updateRequest);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateLearningGoal_WhenOptionalValuesAreNull_ClearsAndPersistsThem()
    {
        using var factory = new KnowrivaWebApplicationFactory();
        using var client = factory.CreateClient();

        var createRequest = new CreateLearningGoalRequest
        {
            Title = "Learn EF Core",
            Description = "Study basic queries",
            TargetDate = new DateOnly(2026, 12, 31)
        };

        using var createResponse = await client.PostAsJsonAsync("/api/learning-goals", createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdGoal = await createResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(createdGoal);
        Assert.Equal(createRequest.Description, createdGoal.Description);
        Assert.Equal(createRequest.TargetDate, createdGoal.TargetDate);

        var updateRequest = new UpdateLearningGoalRequest
        {
            Title = createRequest.Title,
            Description = null,
            TargetDate = null
        };

        var goalUrl = $"/api/learning-goals/{createdGoal.Id}";
        using var updateResponse = await client.PutAsJsonAsync(goalUrl, updateRequest);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedGoal = await updateResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(updatedGoal);
        Assert.Equal(createdGoal.Id, updatedGoal.Id);
        Assert.Equal(createdGoal.Title, updatedGoal.Title);
        Assert.Equal(createdGoal.CreatedAtUtc, updatedGoal.CreatedAtUtc);
        Assert.Null(updatedGoal.Description);
        Assert.Null(updatedGoal.TargetDate);
        Assert.NotNull(updatedGoal.UpdatedAtUtc);

        using var getResponse = await client.GetAsync(goalUrl);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrievedGoal = await getResponse.Content.ReadFromJsonAsync<LearningGoalDto>();
        Assert.NotNull(retrievedGoal);
        Assert.Equal(updatedGoal, retrievedGoal);
    }

}
