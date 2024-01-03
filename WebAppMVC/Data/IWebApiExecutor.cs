
namespace WebAppMVC.Data
{
    public interface IWebApiExecutor
    {
        Task<T?> InvokeGet<T>(string relativeUrl);
        Task<T?> InvokePost<T>(string relativeUrl, T data);
    }
}