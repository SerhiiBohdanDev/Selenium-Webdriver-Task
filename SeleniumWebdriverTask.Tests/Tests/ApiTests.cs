// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Internal;
using RestSharp;
using SeleniumWebdriverTask.CoreLayer.API;
using SeleniumWebdriverTask.CoreLayer.API.Models;

namespace SeleniumWebdriverTask.TestLayer.Tests;

/// <summary>
/// Class containing tests related to API testing.
/// </summary>
internal class ApiTests : BaseTest
{
    private ApiClient _client;

    /// <summary>
    /// Runs before every test.
    /// </summary>
    public override void Setup()
    {
        base.Setup();
        _client = new(Configuration.ApiTestsBaseUrl);
    }

    /// <summary>
    /// Verifies that at least one user exists.
    /// </summary>
    /// <returns>A task object that can be awaited.</returns>
    /// <exception cref="InvalidOperationException">Thrown if can't convert response to users list.</exception>
    [Test]
    [Category("API")]
    public async Task UsersExist_GetUsers_Success()
    {
        var expectedStatus = HttpStatusCode.OK;
        var response = ValidateResponse(await _client.GetUsersAsync(Configuration.UsersEndpoint));
        var responseContent = ValidateContent(response.Content);

        var users = ValidateUsers(JsonConvert.DeserializeObject<List<User>>(responseContent));

        Logger.LogInformation($"ErrorMessage = '{response.ErrorMessage}'");
        Logger.LogInformation($"Users count = {users.Count}");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
            Assert.That(response.ErrorMessage, Is.Null);
            Assert.That(users, Is.Not.Empty);
        }
    }

    /// <summary>
    /// Verifies that expected ContentType header is of correct type.
    /// </summary>
    /// <returns>A task object that can be awaited.</returns>
    [Test]
    [Category("API")]
    public async Task HeaderExists_GetUsers_Success()
    {
        var expectedStatus = HttpStatusCode.OK;
        var expectedHeader = "application/json";
        var response = ValidateResponse(await _client.GetUsersAsync(Configuration.UsersEndpoint));

        Logger.LogInformation($"ErrorMessage = '{response.ErrorMessage}'");
        Logger.LogInformation($"Received ContentType = {response.ContentType}");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
            Assert.That(response.ContentType, Is.EqualTo(expectedHeader));
            Assert.That(response.ErrorMessage, Is.Null);
        }
    }

    /// <summary>
    /// Verifies that users have unique ids, have names and usernames, and have company name.
    /// </summary>
    /// <returns>A task object that can be awaited.</returns>
    /// <exception cref="InvalidOperationException">Thrown if can't convert response to users list.</exception>
    [Test]
    [Category("API")]
    public async Task UsersAreUnique_GetUsers_Success()
    {
        var expectedStatus = HttpStatusCode.OK;
        var expectedUsersAmount = 10;
        var response = ValidateResponse(await _client.GetUsersAsync(Configuration.UsersEndpoint));
        var responseContent = ValidateContent(response.Content);

        var users = ValidateUsers(JsonConvert.DeserializeObject<List<User>>(responseContent));

        var hasDuplicateIds = users.Select(u => u.Id).Distinct().Count() != users.Count;
        var allHaveNameAndUsername = users.All(u => !string.IsNullOrEmpty(u.Name) && !string.IsNullOrEmpty(u.Username));
        var allHaveCompanyName = users.All(u => !string.IsNullOrEmpty(u.Company.Name));

        Logger.LogInformation($"ErrorMessage = '{response.ErrorMessage}'");
        Logger.LogInformation($"Users count = {users.Count}");
        Logger.LogInformation($"Users have duplicate ids = {hasDuplicateIds}");
        Logger.LogInformation($"All users have unique name and username = {allHaveNameAndUsername}");
        Logger.LogInformation($"All users have company name = {allHaveCompanyName}");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
            Assert.That(response.ErrorMessage, Is.Null);
            Assert.That(users, Has.Count.EqualTo(expectedUsersAmount));
            Assert.That(hasDuplicateIds, Is.False);
            Assert.That(allHaveNameAndUsername, Is.True);
            Assert.That(allHaveCompanyName, Is.True);
        }
    }

    /// <summary>
    /// Validates that user is created.
    /// </summary>
    /// <returns>A task object that can be awaited.</returns>
    [Test]
    [Category("API")]
    public async Task ValidData_CreateUser_Success()
    {
        var expectedStatus = HttpStatusCode.Created;
        var dateTime = DateTime.UtcNow.ToString("yyyy_M_dd_hh_mm_ss");
        var name = "Name_" + dateTime;
        var username = "Username_" + dateTime;
        var user =
            new UserDtoBuilder()
            .WithName(name)
            .WithUsername(username)
            .Build();

        var response = ValidateResponse(await _client.CreateUserAsync(user, Configuration.UsersEndpoint));
        var responseContent = ValidateContent(response.Content);

        var userData = JObject.Parse(responseContent);
        var idExists = userData.ContainsKey("id");
        Logger.LogInformation($"User created = {userData}");
        Logger.LogInformation($"ErrorMessage = '{response.ErrorMessage}'");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
            Assert.That(response.ErrorMessage, Is.Null);
            Assert.That(idExists, Is.True);
        }
    }

    /// <summary>
    /// Validates that going to invalid endpoint returs 404.
    /// </summary>
    /// <returns>A task object that can be awaited.</returns>
    [Test]
    [Category("API")]
    public async Task InvalidEndpoint_GetEndpoint_Success()
    {
        var expectedStatus = HttpStatusCode.NotFound;
        var response = ValidateResponse(await _client.GetInvalidEndpointAsync(Configuration.InvalidEndpoint));

        Logger.LogInformation($"ErrorMessage = '{response.ErrorMessage}'");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
            Assert.That(response.ErrorMessage, Is.Null);
        }
    }

    private RestResponse ValidateResponse(RestResponse response)
    {
        if (response == null)
        {
            Logger.LogError("Response is null");
            throw new InvalidOperationException($"Response is null");
        }

        Logger.LogInformation($"Response status = {response.StatusCode}");
        return response;
    }

    private string ValidateContent(string? content)
    {
        if (string.IsNullOrEmpty(content))
        {
            Logger.LogError($"Response content is null or empty: {content}");
            throw new InvalidOperationException($"Response content is null or empty: {content}");
        }

        Logger.LogInformation($"Response content = {content}");
        return content;
    }

    private List<User> ValidateUsers(List<User>? users)
    {
        if (users == null)
        {
            Logger.LogError($"Couldn't deserialize content to appropriate format.");
            throw new InvalidOperationException($"Couldn't deserialize content to appropriate format.");
        }

        return users;
    }
}
