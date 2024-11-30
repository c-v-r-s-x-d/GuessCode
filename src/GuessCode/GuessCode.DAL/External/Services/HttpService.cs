using GuessCode.DAL.External.Contracts;
using GuessCode.DAL.External.Enums;
using Newtonsoft.Json;

namespace GuessCode.DAL.External.Services;

public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public Dictionary<string, string> AdditionalHeaders { get; private set; } = new();

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void AddDefaultRequestHeader(string header, string value)
    {
        if (AdditionalHeaders.TryGetValue(header, out var currentValue))
        {
            if (value == currentValue)
            {
                return;
            }
        }
        AdditionalHeaders.Add(header, value);
    }

    public async Task<T> GetAsync<T>(string uri, CancellationToken cancellationToken)
    {
        var result = await SendRequest(HttpMethodType.Get, uri, cancellationToken: cancellationToken);
        return DeserializeObject<T>(result)!;
    }
    
    public async Task<T> PostAsync<T>(string uri, HttpContent content, CancellationToken cancellationToken)
    {
        var result = await SendRequest(HttpMethodType.Post, uri, content, cancellationToken: cancellationToken);
        return DeserializeObject<T>(result)!;
    }
    
    public async Task<T> PutAsync<T>(string uri, HttpContent content, CancellationToken cancellationToken)
    {
        var result = await SendRequest(HttpMethodType.Put, uri, content, cancellationToken: cancellationToken);
        return DeserializeObject<T>(result)!;
    }
    
    public async Task<T> DeleteAsync<T>(string uri, CancellationToken cancellationToken)
    {
        var result = await SendRequest(HttpMethodType.Delete, uri, cancellationToken: cancellationToken);
        return DeserializeObject<T>(result)!;
    }
    
    private async Task<string> SendRequest(HttpMethodType methodType, string uri, HttpContent? content = null, CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient();
        
        FillHeaders(client, AdditionalHeaders);

        var response = methodType switch
        {
            HttpMethodType.Get => await client.GetAsync(uri, cancellationToken),
            HttpMethodType.Post => await client.PostAsync(uri, content, cancellationToken),
            HttpMethodType.Put => await client.PutAsync(uri, content, cancellationToken),
            HttpMethodType.Delete => await client.DeleteAsync(uri, cancellationToken),
            _ => throw new ArgumentOutOfRangeException($"Incorrect method: {nameof(methodType)}")
        };

        return await ReadResponseResultAsString(response);
    }

    private static void FillHeaders(HttpClient client, IReadOnlyDictionary<string, string>? additionalHeaders)
    {
        if (additionalHeaders is null) return;
        
        foreach (var header in additionalHeaders)
        {
            client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }

    private static Task<string> ReadResponseResultAsString(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Http request failed, status code: {response.StatusCode}");
    }

    //TODO: подумать как убрать костыль
    private static T? DeserializeObject<T>(string value)
    {
        return typeof(T) == typeof(string) ? (T)(object)value : JsonConvert.DeserializeObject<T>(value);
    }
}