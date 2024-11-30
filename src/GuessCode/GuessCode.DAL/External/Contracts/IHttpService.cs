namespace GuessCode.DAL.External.Contracts;

public interface IHttpService
{
    void AddDefaultRequestHeader(string header, string value);
    Task<T> GetAsync<T>(string uri, CancellationToken cancellationToken);
    Task<T> PostAsync<T>(string uri, HttpContent httpContent, CancellationToken cancellationToken);
    Task<T> PutAsync<T>(string uri, HttpContent httpContent, CancellationToken cancellationToken);
    Task<T> DeleteAsync<T>(string uri, CancellationToken cancellationToken);
}