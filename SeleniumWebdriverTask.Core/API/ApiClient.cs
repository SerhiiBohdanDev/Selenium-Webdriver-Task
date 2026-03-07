// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.Json;
using System.Text.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.Json;
using SeleniumWebdriverTask.CoreLayer.API.Models;

namespace SeleniumWebdriverTask.CoreLayer.API;

/// <summary>
/// A class to execute api calls.
/// </summary>
public class ApiClient
{
    private readonly IRestClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient"/> class.
    /// </summary>
    /// <param name="baseUrl">Base url.</param>
    public ApiClient(string baseUrl)
    {
        var serializerOptions = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        _client = new RestClient(
            options: new() { BaseUrl = new(baseUrl) },
            configureSerialization: s => s.UseSystemTextJson(serializerOptions));
    }

    /// <summary>
    /// Sends GET request to endpoint.
    /// </summary>
    /// <param name="endpoint">Endpoint to send request to.</param>
    /// <returns>RequestResponse.</returns>
    public async Task<RestResponse> CallGetAsync(string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Get);
        var response = await _client.GetAsync(request);

        return response;
    }

    /// <summary>
    /// Sends request to create a user.
    /// </summary>
    /// <param name="user">Instance of User object.</param>
    /// <param name="endpoint">Endpoint containing users data.</param>
    /// <returns>RestRequest with information about request state and user data.</returns>
    public async Task<RestResponse> CreateUserAsync(User user, string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Post);
        request.AddBody(new { user.Name, user.Username, });
        var response = await _client.PostAsync(request);

        return response;
    }
}
