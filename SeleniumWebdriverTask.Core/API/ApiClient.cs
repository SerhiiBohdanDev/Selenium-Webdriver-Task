// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using System.Text.Json;
using System.Text.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.Json;

namespace SeleniumWebdriverTask.CoreLayer.API;

public class ApiClient
{
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

    public async Task<RestResponse> GetUsersAsync()
    {
        var request = new RestRequest($"/users", Method.Get);
        var response = await _client.GetAsync(request);

        return response;
    }

    public async Task<RestResponse> CreateUserAsync(string name, string username)
    {
        var request = new RestRequest($"/users", Method.Post);
        request.AddBody(new { name, username, });
        var response = await _client.PostAsync(request);

        return response;
    }

    public async Task<RestResponse> GetInvalidEndpointAsync()
    {
        var request = new RestRequest($"/invalidendpoint", Method.Get);
        var response = await _client.GetAsync(request);

        return response;
    }
}
