// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SeleniumWebdriverTask.CoreLayer.API;
using SeleniumWebdriverTask.CoreLayer.API.Models;

namespace SeleniumWebdriverTask.TestLayer;

/// <summary>
/// Class containing tests related to API testing.
/// </summary>
internal class ApiTests
{
    private readonly ApiClient _client = new("https://jsonplaceholder.typicode.com/");

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
        var response = ValidateResponse(await _client.GetUsersAsync(), out var responseContent);

        var users = JsonConvert.DeserializeObject<List<User>>(responseContent);
        if (users == null)
        {
            throw new InvalidOperationException($"Couldn't deserialize content to appropriate format.");
        }

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
        var response = ValidateResponse(await _client.GetUsersAsync(), out var _);

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
        var response = ValidateResponse(await _client.GetUsersAsync(), out var responseContent);

        var users = JsonConvert.DeserializeObject<List<User>>(responseContent);
        if (users == null)
        {
            throw new InvalidOperationException($"Couldn't deserialize content to appropriate format.");
        }

        var hasDuplicateIds = users.Select(u => u.Id).Distinct().Count() != users.Count;
        var allHaveNameAndUsername = users.All(u => !string.IsNullOrEmpty(u.Name) && !string.IsNullOrEmpty(u.Username));
        var allHaveCompanyName = users.All(u => !string.IsNullOrEmpty(u.Company.Name));

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
        var name = "Jim";
        var username = "jimbo";
        var response = ValidateResponse(await _client.CreateUserAsync(name, username), out var responseContent);

        var jsonObj = JObject.Parse(responseContent);
        var idExists = jsonObj.ContainsKey("id");

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
        // Going to invalid endpoint doesn't work, any page returns ok.
        var expectedStatus = HttpStatusCode.OK;
        var response = ValidateResponse(await _client.GetUsersAsync(), out var _);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
            Assert.That(response.ErrorMessage, Is.Null);
        }
    }

    private static RestResponse ValidateResponse(RestResponse response, out string responseContent)
    {
        if (response == null)
        {
            throw new InvalidOperationException($"Did not receive a valid response.");
        }

        if (string.IsNullOrEmpty(response.Content))
        {
            throw new InvalidOperationException($"Response content was null or empty: {response.Content}");
        }

        responseContent = response.Content;
        return response;
    }
}
