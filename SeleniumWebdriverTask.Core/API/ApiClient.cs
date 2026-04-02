// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using RestSharp;
using SeleniumWebdriverTask.CoreLayer.API.Builders;
using SeleniumWebdriverTask.CoreLayer.API.Models;
using SeleniumWebdriverTask.CoreLayer.Configurations;

namespace SeleniumWebdriverTask.CoreLayer.API;

/// <summary>
/// A class to execute api calls.
/// </summary>
public class ApiClient
{
    private readonly IRestClient _client;
    private readonly ApiEndpoints _endpoints;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient"/> class.
    /// </summary>
    /// <param name="endpoints">Struct containing endpoints for API calls.</param>
    public ApiClient(ApiEndpoints endpoints)
    {
        _endpoints = endpoints;

        _client = new RestClient(
            options: new() { BaseUrl = new(_endpoints.BaseUrl) });
    }

    /// <summary>
    /// Accesses invalid endpoint.
    /// </summary>
    /// <returns>A task representing the asynchronous operation with RestResponse data.</returns>
    public async Task<RestResponse> AccessInvalidEndpoint()
    {
        var request =
            new RestRequestBuilder(_endpoints.InvalidEndpoint)
            .Build();

        return await ExecuteAsync(request);
    }

    /// <summary>
    /// Creates user.
    /// </summary>
    /// <param name="name">Name of the user.</param>
    /// <param name="username">Username of the user.</param>
    /// <returns>A task representing the asynchronous operation with RestResponse data.</returns>
    public async Task<RestResponse<User>> CreateUser(string name, string username)
    {
        var user =
            new UserDtoBuilder()
            .WithName(name)
            .WithUsername(username)
            .Build();

        var request =
            new RestRequestBuilder(_endpoints.UsersEndpoint, Method.Post)
            .AddJsonBody(user)
            .Build();

        return await ExecuteAsync<User>(request);
    }

    /// <summary>
    /// Gets users.
    /// </summary>
    /// <returns>A task representing the asynchronous operation with RestResponse data.</returns>
    public async Task<RestResponse<User[]>> GetUsers()
    {
        var request =
            new RestRequestBuilder(_endpoints.UsersEndpoint)
            .Build();

        return await ExecuteAsync<User[]>(request);
    }

    private async Task<RestResponse> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.ExecuteAsync(request, cancellationToken);
    }

    private async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
        where T : class
    {
        var response = await _client.ExecuteAsync<T>(request, cancellationToken);
        return response;
    }
}
