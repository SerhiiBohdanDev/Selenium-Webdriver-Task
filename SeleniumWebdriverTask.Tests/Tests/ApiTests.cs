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
    [Test]
    [Category("API")]
    public async Task UsersExist_GetUsers_Success()
    {
        var expectedStatus = HttpStatusCode.OK;

        var response = await _client.CallGetAsync(Configuration.UsersEndpoint);
        LogResponseStatus(response);
        var responseContent = ValidateContent(response.Content);

        var users = ValidateUsers(responseContent);

        using (Assert.EnterMultipleScope())
        {
            AssertResponseIsValid(response, expectedStatus);
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
        var expectedContentType = "application/json";

        var response = await _client.CallGetAsync(Configuration.UsersEndpoint);
        LogResponseStatus(response);

        Logger.LogInformation($"Received ContentType = {response.ContentType}");
        using (Assert.EnterMultipleScope())
        {
            AssertResponseIsValid(response, expectedStatus);
            Assert.That(response.ContentType, Is.EqualTo(expectedContentType));
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

        var response = await _client.CallGetAsync(Configuration.UsersEndpoint);
        LogResponseStatus(response);

        var responseContent = ValidateContent(response.Content);
        var users = ValidateUsers(responseContent);

        var duplicateUsers = users
            .GroupBy(u => u.Id)
            .Where(g => g.Count() > 1)
            .SelectMany(g => g)
            .ToList();

        var usersMissingNameOrUsername = users
            .Where(u => string.IsNullOrEmpty(u.Name) || string.IsNullOrEmpty(u.Username))
            .ToList();
        var usersMissingCompanyName = users
            .Where(u => string.IsNullOrEmpty(u.Company.Name))
            .ToList();

        LogInvalidUsers(duplicateUsers, usersMissingNameOrUsername, usersMissingCompanyName);
        using (Assert.EnterMultipleScope())
        {
            AssertResponseIsValid(response, expectedStatus);
            Assert.That(users, Has.Length.EqualTo(expectedUsersAmount));
            Assert.That(duplicateUsers, Has.Count.Zero);
            Assert.That(usersMissingNameOrUsername, Has.Count.Zero);
            Assert.That(usersMissingCompanyName, Has.Count.Zero);
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
        var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd_hh_mm_ss");
        var name = "Name_" + dateTime;
        var username = "Username_" + dateTime;
        var user =
            new UserDtoBuilder()
            .WithName(name)
            .WithUsername(username)
            .Build();

        var response = await _client.CreateUserAsync(user, Configuration.UsersEndpoint);
        LogResponseStatus(response);

        var responseContent = ValidateContent(response.Content);
        var userData = JObject.Parse(responseContent);
        var idExists = userData.ContainsKey("id");

        if (response.StatusCode == expectedStatus)
        {
            Logger.LogInformation($"User created = {userData}");
        }

        using (Assert.EnterMultipleScope())
        {
            AssertResponseIsValid(response, expectedStatus);
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

        var response = await _client.CallGetAsync(Configuration.InvalidEndpoint);
        LogResponseStatus(response);

        AssertResponseIsValid(response, expectedStatus);
    }

    private static void AssertResponseIsValid(RestResponse response, HttpStatusCode expectedStatus)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
            Assert.That(response.ErrorMessage, Is.Null);
        }
    }

    private void LogResponseStatus(RestResponse response)
    {
        Logger.LogInformation($"Response status = {response.StatusCode}");
        Logger.LogInformation($"ErrorMessage = '{response.ErrorMessage}'");
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

    private User[] ValidateUsers(string responseContent)
    {
        User[]? users;
        try
        {
            users = JsonConvert.DeserializeObject<User[]>(responseContent);
        }
        catch (JsonSerializationException ex)
        {
            Logger.LogError(ex.Message);
            throw;
        }

        if (users == null)
        {
            throw new InvalidOperationException($"Deserialization resulted in null. The input JSON may be invalid or empty.");
        }

        return users;
    }

    private void LogInvalidUsers(List<User> duplicateUsers, List<User> usersMissingNameOrUsername, List<User> usersMissingCompanyName)
    {
        for (int i = 0; i < duplicateUsers.Count; i++)
        {
            var user = duplicateUsers[i];
            Logger.LogError($"User has duplicate id: {user}");
        }

        for (int i = 0; i < usersMissingNameOrUsername.Count; i++)
        {
            var user = usersMissingNameOrUsername[i];
            Logger.LogError($"User is missing name or username: {user}");
        }

        for (int i = 0; i < usersMissingCompanyName.Count; i++)
        {
            var user = usersMissingCompanyName[i];
            Logger.LogError($"User is missing company name: {user}");
        }
    }
}
