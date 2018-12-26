using System.Net.Http;
using System.Threading.Tasks;

namespace Coligo.ReachMee.Data.Interfaces
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PutAsync(string url, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(string url);
    }
}
