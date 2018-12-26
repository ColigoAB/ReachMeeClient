using Coligo.ReachMee.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Coligo.ReachMee.Data.ApiClients
{
    public class ReachMeeClient : IApiClient
    {
        #region Private fields
        private readonly HttpClient _httpClient;
        #endregion

        #region Constructors

        public ReachMeeClient(Uri apiUrl, string apiUserName = null, string apiKey = null)
        {
            if (apiUrl == null) throw new ArgumentNullException(nameof(apiUrl));

            string apiAuthString = null;
            if (apiUserName != null && apiKey != null)
            {
                apiAuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiUserName}:{apiKey}"));
            }
            _httpClient = GetApiHttpClient(apiUrl, apiAuthString);
        }

        public ReachMeeClient(Uri apiUrl, string apiAuthString = null)
        {
            _httpClient = GetApiHttpClient(apiUrl, apiAuthString);
        }
        #endregion

        #region Public Methods
        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content,

            };

            return await this.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            return await this.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = content,

            };

            return await this.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);

            return await this.SendAsync(request);
        }
        #endregion

        #region Private Methods
        private static HttpClient GetApiHttpClient(Uri apiUrl, string apiAuthString)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new
                        MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrWhiteSpace(apiAuthString)) httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiAuthString);
                httpClient.BaseAddress = apiUrl;
                return httpClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            try
            {
                return await _httpClient.SendAsync(request);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
