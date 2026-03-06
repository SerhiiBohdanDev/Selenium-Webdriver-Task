// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.Json;
using System.Text.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.Json;

namespace SeleniumWebdriverTask.CoreLayer.API;

/// <summary>
/// A class to execute api calls.
/// </summary>
public class ApiClient
{
    private const string UsersEndpoint = "/users";
    private const string InvalidEndpoint = "/invalidendpoint";

    private readonly IRestClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient"/> class.
    /// </summary>
    /// <param name="endpoint">Endpoint url.</param>
    public ApiClient(string endpoint)
    {
        var serializerOptions = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        _client = new RestClient(
            options: new() { BaseUrl = new(endpoint) },
            configureSerialization: s => s.UseSystemTextJson(serializerOptions));
    }

    /// <summary>
    /// Sends request to get users information.
    /// </summary>
    /// <returns>RequestResponse containing information about users.</returns>
    public async Task<RestResponse> GetUsersAsync()
    {
        var request = new RestRequest(UsersEndpoint, Method.Get);
        var response = await _client.GetAsync(request);

        return response;
    }

    /// <summary>
    /// Sends request to create a user.
    /// </summary>
    /// <param name="name">User's name.</param>
    /// <param name="username">User's username.</param>
    /// <returns>RestRequest with information about request state and user data.</returns>
    public async Task<RestResponse> CreateUserAsync(string name, string username)
    {
        var request = new RestRequest(UsersEndpoint, Method.Post);
        request.AddBody(new { name, username, });
        var response = await _client.PostAsync(request);

        return response;
    }

    /// <summary>
    /// Sends request to invalid endpoint.
    /// </summary>
    /// <returns>RestResponse with information about request state.</returns>
    public async Task<RestResponse> GetInvalidEndpointAsync()
    {
        var request = new RestRequest(InvalidEndpoint, Method.Get);
        var response = await _client.GetAsync(request);

        return response;
    }
}
