// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.:suggestion

using RestSharp;

namespace SeleniumWebdriverTask.CoreLayer.API.Builders;

/// <summary>
/// A class to build requests.
/// </summary>
public sealed class RestRequestBuilder
{
    private readonly RestRequest _request;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestRequestBuilder"/> class.
    /// </summary>
    /// <param name="resource">Relative resource url.</param>
    /// <param name="method">HTTP method for request.</param>
    public RestRequestBuilder(string resource, Method method = Method.Get)
    {
        _request = new RestRequest(resource, method);
    }

    /// <summary>
    /// Adds a JSON body parameter to the request.
    /// </summary>
    /// <typeparam name="T">A class that can be serialized to JSON.</typeparam>
    /// <param name="body">Object that will be serialized to JSON.</param>
    /// <returns>Instance of RestRequestBuilder.</returns>
    public RestRequestBuilder AddJsonBody<T>(T body)
        where T : class
    {
        _request.AddJsonBody(body);
        return this;
    }

    /// <summary>
    /// Builds the request.
    /// </summary>
    /// <returns>Instance of RestRequest.</returns>
    public RestRequest Build() => _request;
}
