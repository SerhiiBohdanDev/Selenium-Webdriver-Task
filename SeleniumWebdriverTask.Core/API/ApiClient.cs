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
    /// Executes the request asynchronously, authenticating if needed.
    /// </summary>
    /// <param name="request">Request to be executed.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation with RestResponse data.</returns>
    public async Task<RestResponse> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.ExecuteAsync(request, cancellationToken);
    }
}
