using WebshopFrontend.Services;

namespace WebshopFrontend.Contracts;

public interface IApiService
{
    Task<Result<TOutput>> GetAsync<TOutput>(string url);
    Task<Result<TOutput>> PostAsync<TOutput, TInput>(string url, TInput data);
    Task<Result<TOutput>> PutAsync<TOutput, TInput>(string url, TInput data);
    Task<Result<TOutput>> DeleteAsync<TOutput>(string url);
}